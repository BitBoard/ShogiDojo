using System;
using Cysharp.Threading.Tasks;
using MyShogi.Model.Shogi.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour
{
	[SerializeField] private GameSceneView view;
	[SerializeField] private GameObject piecePrefab;
	[SerializeField] private ConfigPopupController configPopupController;
    private const int boardSize = 81;
    private const int boardColumns = 9;
    private const int boardRows = 9;
    private Cell[,] cells;
    private Piece selectedPiece = null;
    private GameState gameState;
    private IShogiAI battleAI;
    private bool isBlackTurn = true;
    private bool shouldPromote = false;
    private bool promoteSelectionDone = false;
	private CapturePieceAreaData capturePieceAreaData;

	private void Start()
    { 
	    Init();
    }

	private void Init()
	{
        SetEvent();
        SetCells();
	}
	
	private void SetEvent()
	{
		view.OpenDebugMenuButton.onClick.AddListener(OpenDebugMenu);
		view.CloseDebugMenuButton.onClick.AddListener(CloseDebugMenu);
		configPopupController.action += new UnityAction<string, BoardType>((boardJsonPath, boardType) =>
        {
            UniTask.Void(async () => await InitBoard(boardJsonPath, boardType));
        });
		view.PromotePopupView.PromoteButton.onClick.AddListener(() =>
		{
			shouldPromote = true;
			view.PromotePopupView.gameObject.SetActive(false);
			promoteSelectionDone = true;
		});
		view.PromotePopupView.CancelPromoteButton.onClick.AddListener(() =>
		{
			shouldPromote = false;
			view.PromotePopupView.gameObject.SetActive(false);
			promoteSelectionDone = true;
		});
		
		view.RetryButton.onClick.AddListener(() =>
		{
			view.ResultPanel.SetActive(false);
			UniTask.Void(async () => await InitBoard());
		});
		
		view.ResetButton.onClick.AddListener(() =>
		{
			// シーンをリロードする
			UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
		});
	}

	private void SetCells()
	{
		var cellArray = view.Board.GetComponentsInChildren<Cell>();
		if(cellArray.Length != boardSize)
		{
			throw new Exception("将棋盤のマス目は81マスにしてください");
		}

		var x = 0;
		var y = 0;
		cells = new Cell[boardRows, boardColumns];

        for (int i = 0; i < boardSize; i++)
        {
            cells[y, x] = cellArray[i];
            cells[y, x].x = x;
            cells[y, x].y = y;
            var cell = cells[y, x];
            cell.OnClickAction += UniTask.UnityAction(async () =>
            {
	            if (selectedPiece != null)
	            {
		            if (!IsPlayerTurn())
		            {
			            return;
		            }
		            await DoAction(cell);
	            }
            });

            if((i+1) % boardColumns == 0)
            {
                y++;
				x = 0;
			} else
			{
				x++;
			}
        }
    }

	/// <summary>
	/// 盤面の初期化を行う
	/// </summary>
	/// <param name="boardJsonPath"></param>
	/// <param name="boardType"></param>
	private async UniTask InitBoard(string boardJsonPath = "", BoardType boardType = BoardType.NoHandicap)
    {
        ClearPieces();
        ClearHighlight();
		view.ClearAllCapturePieceArea();
        capturePieceAreaData = new CapturePieceAreaData();
        selectedPiece = null;
        gameState = new GameState(boardType);
        gameState.ShowBoard();
        battleAI = new RandomAI();
        isBlackTurn = true;


        if (String.IsNullOrEmpty(boardJsonPath))
		{
			boardJsonPath = GameConfig.initialBoardJsonPath;
        }
		var json = Resources.Load<TextAsset>(boardJsonPath).ToString();
        var boardData = JsonUtility.FromJson<BoardData>(json);

		Debug.Log(boardData);
        foreach (var data in boardData.boardData)
        {
            var cellX = GameConfig.isAIFirst && !GameConfig.isDropPiece ? 8 - data.x : data.x;
            var cellY = GameConfig.isAIFirst && !GameConfig.isDropPiece ? 8 - data.y : data.y;
            var piece = Instantiate(piecePrefab, cells[cellY, cellX].transform);
            piece.GetComponent<Piece>().SetPieceType(PieceData.StrToPieceType(data.pieceType));
			piece.GetComponent<Piece>().SetPiecePosition(cellX, cellY);
            
			
			piece.GetComponent<Piece>().OnClickAction += UniTask.UnityAction(async () =>
			{
				if (!IsPlayerTurn())
				{
					return;
				}
				await DoAction(piece.GetComponent<Piece>());
			});
			
	    }

        if (GameConfig.isDropPiece)
        {
	        isBlackTurn = false;
        }

		if (GameConfig.isAIFirst)
		{
			await GetAIAction();
		}
    }

	/// <summary>
	/// 盤上の駒を全て消去する
	/// </summary>
	private void ClearPieces()
	{
		foreach (var cell in cells)
		{
			var piece = cell.GetComponentInChildren<Piece>()?.gameObject;
			if (piece != null)
			{
				Destroy(piece);
			}
		}
	}
	
	/// <summary>
	/// 全てのハイライトを消去する
	/// </summary>
	private void ClearHighlight()
	{
		foreach (var cell in cells)
		{
			cell.SetHighlight(false);
		}
	}
	
	/// <summary>
	/// 駒を選択状態にする
	/// </summary>
	/// <param name="piece"></param>
	private void SetSelectedPiece(Piece piece)
	{
		selectedPiece = piece;
		selectedPiece.SetIsShowOutline(true);
		Debug.Log("選択した駒:" + piece);
	}
	
	/// <summary>
	/// 駒の選択状態を解除する
	/// </summary>
	private void ClearSelectedPiece()
	{
		if (selectedPiece != null)
		{
			selectedPiece.SetIsShowOutline(false);
			selectedPiece = null;
		}
	}

	/// <summary>
	/// 駒またはマスをクリックした時の処理
	/// </summary>
	/// <param name="target"></param>
	private async UniTask DoAction(ISelectableTarget target)
	{
		// 選択されている駒がない場合
		if (selectedPiece == null)
		{
			// targetが駒の場合
			if (target is Piece piece)
			{
				if (!piece.IsTurnPlayerPiece(isBlackTurn))
				{
					Debug.Log("自分の駒を選択してください");
					return;
				}
				// 駒を選択する
				SetSelectedPiece(piece);
			}
			
			return;
		}
		
		// 駒が選択されている場合、合法手かどうかの判定に入る
		Move move = Move.NONE;
		Move movePromote = Move.NONE;
		Move decidedMove = Move.NONE;

		// 持ち駒を打つ場合
		if (selectedPiece.IsCaptured())
		{
			// 持ち駒を他の駒がある位置に置こうとした場合
			if (target is Piece)
			{
				Debug.Log("不正な手です");
				ClearSelectedPiece();
				return;
			}
			var pt = Converter.PieceTypeToDropPiece(selectedPiece.GetPieceType());
			var to = target.SqPos();
			move = Util.MakeMoveDrop(pt, to);
			decidedMove = move;
		}
		else
		{
			// 盤上の駒を移動する場合
			var from = selectedPiece.SqPos();
			var to = target.SqPos();
			move = Util.MakeMove(from, to);
			movePromote = Util.MakeMovePromote(from, to);
			decidedMove = move;
		}
		
		// 合法手かどうかを判定する
		Debug.Log("指し手:" + move.Pretty());
		if (!gameState.IsValidMove(move))
		{
			// 成りが合法手の場合
			if (gameState.IsValidMove(movePromote))
			{
				// この場合は強制的に成る
				decidedMove = movePromote;
				selectedPiece.Promotion();
			}
			else
			{
				Debug.Log("不正な手です");
				ClearSelectedPiece();
				return;
			}
		}
		
		// 成りも不成も選択できる場合
		if (gameState.IsValidMove(move) && gameState.IsValidMove(movePromote))
		{
			// 成るかどうかを選択する
			if (!promoteSelectionDone)
			{
				view.PromotePopupView.gameObject.SetActive(true);
			}

			// ここで待機する
			await UniTask.WaitUntil(() => promoteSelectionDone);
			
			Debug.Log("成るかどうか:" + shouldPromote);
			
			if (shouldPromote)
			{
				decidedMove = movePromote;
				selectedPiece.Promotion();
			}
			
			// 変数を初期化
			shouldPromote = false;
			promoteSelectionDone = false;
		}

		switch (target)
		{
			case Piece pieceObject:
				// 選択されている駒を移動させる
				selectedPiece.transform.SetParent(pieceObject.transform.parent);
				// 移動先のマスの駒を取る
				CapturePiece(pieceObject);
				// 駒の位置を更新する
				selectedPiece.transform.localPosition = Vector3.zero;
				var pos = pieceObject.GetPiecePosition();
				selectedPiece.SetPiecePosition(pos.x, pos.y);
				break;
			case Cell cellObject:
                // 選択されている駒を移動させる
                selectedPiece.transform.SetParent(cellObject.transform);
                // 持ち駒が消費されていた場合は持ち駒情報を更新する
                if (selectedPiece.IsCaptured())
                {
                    UpdateCapturePieceArea(selectedPiece);

                    // 駒数表示を非有効化
                    selectedPiece.SetPieceNum(0);
                }
                // 駒の位置を更新する
                selectedPiece.transform.localPosition = Vector3.zero;
				selectedPiece.SetPiecePosition(cellObject.x, cellObject.y);
				break;
		}
		
		// 駒音を再生する
		selectedPiece.GetComponent<AudioSource>().Play();
		
		// 全てのマスのハイライトをOFFにする
		ClearHighlight();
		// 最終手が着手された位置のcellを取得
		var lastCell = cells[selectedPiece.GetPiecePosition().y, selectedPiece.GetPiecePosition().x];
		// セルのハイライトをONにする
		lastCell.SetHighlight(true);

		Debug.Log("移動した駒:" + selectedPiece);
		
		// 局面を進める
		gameState.Advance(decidedMove);
		gameState.ShowBoard();
		
		// 着手後処理
		ClearSelectedPiece();
		ChangeTurn();
		
		// 詰んでいるかどうかを判定する
		if (gameState.IsMated())
		{
			ShowResult(!IsPlayerTurn());
			return;
		}

		// AIの手番の場合はAIの手を待つ
		if (!IsPlayerTurn())
		{
			await GetAIAction();
			
			// 詰んでいるかどうかを判定する
			if (gameState.IsMated())
			{
				ShowResult(!IsPlayerTurn());
			}
		}
		
	}

	/// <summary>
	/// AIによる着手を行う
	/// </summary>
	private async UniTask GetAIAction()
	{
		await UniTask.Delay(2000);
		var move = battleAI.GetMove(gameState);
		Debug.Log("AIの指し手:" + move.Pretty());
		var from = move.IsDrop() ? Square.NB : move.From();
		var fromX = move.IsDrop() ? -1 : Converter.SquareToX(from);
		var fromY = move.IsDrop() ? -1 : Converter.SquareToY(from);
		var to = move.To();
		var toX = Converter.SquareToX(to);
		var toY = Converter.SquareToY(to);

		Debug.Log("fromX:" + fromX + " fromY:" + fromY + " toX:" + toX + " toY:" + toY);

		// AIが先手の場合は座標を反転させる
		if(!IsPlayerBlack())
		{
			fromX = 8 - fromX;
			fromY = 8 - fromY;
			toX = 8 - toX;
			toY = 8 - toY;
		}

		// 駒を打つ場合
		if (move.IsDrop())
		{
			var pieceType = Converter.DropPieceToPieceType(move.DroppedPiece());
			Debug.Log("打つ駒:" + pieceType);
			selectedPiece = GetCapturedPiece(pieceType);
			await DoAction(cells[toY, toX]);
			return;
		}
		
		// 駒を動かす場合
		selectedPiece = GetPieceOnBoard(fromX, fromY);

		// 駒が成る場合
		if (move.IsPromote())
		{
			shouldPromote = true;
		}
		
		// 成り不成を選択完了とする
		promoteSelectionDone = true;
		
		//移動先のマスにある駒を取得
		var piece = GetPieceOnBoard(toX, toY);
		ISelectableTarget target = (piece != null) ? piece : cells[toY, toX];
		await DoAction(target);

		// 変数を初期化
		shouldPromote = false;
		promoteSelectionDone = false;
	}

/// <summary>
/// 駒を取る処理
/// </summary>
/// <param name="piece"></param>
    private void CapturePiece(Piece piece)
	{
		UpdateCapturePieceArea(piece);

        // 取った駒を消去する
        Destroy(piece.gameObject);
    }

    /// <summary>
    /// 盤上の駒を取得する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Piece GetPieceOnBoard(int x, int y)
	{
		var cell = cells[y, x];
		var piece = cell.GetComponentInChildren<Piece>();
		return piece;
	}
	
	/// <summary>
	/// 駒台の駒を取得する
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	private Piece GetCapturedPiece(PieceType pieceType)
	{
		var capturePieceArea = IsPlayerTurn() ? view.FrontCapturePieceArea : view.BackCapturePieceArea;
		// 駒台にある駒で指定されたPieceTypeのものを取得
		var pieceList = capturePieceArea.GetComponentsInChildren<Piece>();
		foreach (var piece in pieceList)
		{
			if (piece.GetPieceType() == pieceType)
			{
				return piece;
			}
		}
		
		return null;
	}

	/// <summary>
	/// ターンの変更を行う
	/// </summary>
	private void ChangeTurn()
	{
		isBlackTurn = !isBlackTurn;
	}
	
	private bool IsPlayerTurn()
	{
		if (GameConfig.isDropPiece)
		{
			// 駒落ちの場合
			return isBlackTurn;
		}
		// 「先手のターンかつAIが後手」「後手のターンかつAIが先手」の場合、プレイヤーは手番を持っている
		return isBlackTurn != GameConfig.isAIFirst;
	}

    private bool IsPlayerBlack()
    {
	    if (GameConfig.isDropPiece)
	    {
		    // 駒落ちの場合
		    return true;
	    }
        // プレイヤーの持つ駒のPieceTypeがBlackかWhiteかを判定
        return !GameConfig.isAIFirst;
    }

    private bool IsUpdateBlack()
    {
	    // 更新したい情報に関連するPieceTypeがBlackに紐づいているかWhiteに紐づいているかを判定
		return IsPlayerBlack() == IsPlayerTurn();
    }

    private void UpdateCapturePieceArea(Piece piece)
	{
		// 持ち駒情報を更新
		capturePieceAreaData.UpdateCapturePieceData(piece.GetPieceType(), IsUpdateBlack(), selectedPiece.IsCaptured());
        Debug.Log("持ち駒情報の更新 \n" + capturePieceAreaData);

        var capturePieceArea = IsPlayerTurn() ? view.FrontCapturePieceArea : view.BackCapturePieceArea;

        // 最新のcapturePieceAreaDataを反映する前に古い持ち駒の表示を削除
        view.ClearCapturePieceArea(capturePieceArea.transform);

        // 駒台の表示を最新化
        UpdateCapturePieceAreaUI(capturePieceArea);
    }

	private void UpdateCapturePieceAreaUI(GameObject capturePieceArea)
    {
        // 駒台を更新
        foreach (PieceType pt in PieceData.GetPieceTypeList(IsUpdateBlack()))
        {
            var pieceNum = capturePieceAreaData.GetPieceNum(pt, IsUpdateBlack());
            if (pieceNum == 0) continue;

            var pieceByPrefab = Instantiate(piecePrefab, capturePieceArea.transform);
			capturePieceAreaData.CreateCapturePiece(pieceNum, pt, pieceByPrefab);
            pieceByPrefab.GetComponent<Piece>().OnClickAction += UniTask.UnityAction(async () =>
            {
                if (!IsPlayerTurn())
                {
                    return;
                }
                await DoAction(pieceByPrefab.GetComponent<Piece>());
            });
        };
    }
	
	/// <summary>
	/// 結果表示を行う
	/// </summary>
	/// <param name="isPlayerWin"></param>
	public void ShowResult(bool isPlayerWin)
	{
		view.SetResult(isPlayerWin);
		view.ResultPanel.SetActive(true);
	}

	public void OpenDebugMenu()
	{
#if DEVELOPMENT_BUILD || UNITY_EDITOR
		Debug.Log("デバッグメニューを開く");
		view.DebugMenu.SetActive(true);
#endif
	}
	
	public void CloseDebugMenu()
	{
#if DEVELOPMENT_BUILD || UNITY_EDITOR
		Debug.Log("デバッグメニューを閉じる");
		view.DebugMenu.SetActive(false);
#endif
	}
}