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
    private int boardSize = 81;
    private int boardColumns = 9;
    private int boardRows = 9;
    private Cell[,] cells;
    private Piece selectedPiece = null;
    private bool isPieceSelected = false;
    private GameState gameState;
    private IShogiAI battleAI;
    private bool isBlackTurn = true;
    private bool isAIFirst = false;
    private bool shouldPromote = false;
    private bool promoteSelectionDone = false;
	private CapturePieceAreaData capturePieceAreaData;

    private void Awake()
	{
		Init();
	}
	
	private async void Start()
    {
       await InitBoard();
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
		configPopupController.action += new UnityAction<string, BoardType, bool>((boardJsonPath, boardType, isBlackTurn) =>
        {
            UniTask.Void(async () => await InitBoard(boardJsonPath, boardType, isBlackTurn));
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
		            await MovePiece(cell, isBlackTurn);
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

	private async UniTask InitBoard(string boardJsonPath = "", BoardType boardType = BoardType.NoHandicap, bool isAIFirst = false)
    {
        ClearPieces();
		view.ClearAllCapturePieceArea();
        capturePieceAreaData = new CapturePieceAreaData();
        isPieceSelected = false;
        selectedPiece = null;
        gameState = new GameState(boardType);
        gameState.ShowBoard();
        battleAI = new RandomAI();
        this.isBlackTurn = true;
		this.isAIFirst = isAIFirst;

        if (String.IsNullOrEmpty(boardJsonPath))
		{
			boardJsonPath = GameConfig.initialBoardJsonPath;
        }
		var json = Resources.Load<TextAsset>(boardJsonPath).ToString();
        var boardData = JsonUtility.FromJson<BoardData>(json);

		Debug.Log(boardData);
        foreach (var data in boardData.boardData)
        {
            var cellX = isAIFirst ? 8 - data.x : data.x;
            var cellY = isAIFirst ? 8 - data.y : data.y;
            var piece = Instantiate(piecePrefab, cells[cellY, cellX].transform);
			if(isAIFirst)
            {
				var pieceType = "";
				if (data.pieceType.Contains("white"))
				{
					pieceType = data.pieceType.Replace("white", "black");
				} else
				{
                    pieceType = data.pieceType.Replace("black", "white");
                }
                piece.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + pieceType);
            }
			else
            {
                piece.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + data.pieceType);
            }
            piece.GetComponent<Piece>().pieceType = PieceData.StrToPieceType(data.pieceType);
			piece.GetComponent<Piece>().piecePotition = new PieceData.PiecePotition(cellX, cellY);
            
			
			piece.GetComponent<Piece>().OnClickAction += UniTask.UnityAction(async () =>
			{
				if (!IsPlayerTurn())
				{
					return;
				}
				await SelectPiece(piece.GetComponent<Piece>());
			});
			
	    }

		if (isAIFirst)
		{
			await GetAIAction();
		}
    }

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
	/// 駒を選択する処理
	/// note: 相手の駒を取る時はこちらの処理が呼ばれる
	/// </summary>
	/// <param name="piece"></param>
	private async UniTask SelectPiece(Piece piece)
	{
		// そもそも駒が選択されていない場合
		if (selectedPiece == null)
		{
			if (!piece.IsTurnPlayerPiece(isBlackTurn))
			{
				Debug.Log("自分の駒を選択してください");
				
				return;
			}
			// 駒を選択する
			selectedPiece = piece;
			selectedPiece.Outline.SetActive(true);
			isPieceSelected = true;
			Debug.Log("選択した駒:" + piece.ToString());
			return;
		}

		// 持ち駒を他の駒がある位置に置こうとした場合
		if (selectedPiece.IsCaptured())
		{
			Debug.Log("不正な手です");
			isPieceSelected = false;
			selectedPiece.Outline.SetActive(false);
			selectedPiece = null;
			return;
		}
		
		var from = selectedPiece.SqPos(isAIFirst);
		var to = piece.SqPos(isAIFirst);
		
		// 合法手かどうかを判定する
		var move = Util.MakeMove(from, to);
		var movePromote = Util.MakeMovePromote(from, to);
		var decidedMove = move;
		
		Debug.Log("指し手:" + move.Pretty());
		if (!gameState.IsValidMove(move))
		{
			// 成りが合法手の場合
			if (gameState.IsValidMove(movePromote))
			{
				// この場合は強制的に成る
				decidedMove = movePromote;
				PromotePiece(selectedPiece, isAIFirst);
			}
			else
			{
				Debug.Log("不正な手です");
				isPieceSelected = false;
				selectedPiece.Outline.SetActive(false);
				selectedPiece = null;
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
				PromotePiece(selectedPiece, isAIFirst);
			}
			
			// 変数を初期化
			shouldPromote = false;
			promoteSelectionDone = false;
		}
		

		// 選択されている駒を移動させる
		selectedPiece.transform.SetParent(piece.transform.parent);
		// 移動先のマスの駒を取る
		CapturePiece(piece);
		// 駒の位置を更新する
		selectedPiece.transform.localPosition = Vector3.zero;
		selectedPiece.piecePotition = new PieceData.PiecePotition(piece.piecePotition.x, piece.piecePotition.y);
		
		// 駒音を再生する
		selectedPiece.GetComponent<AudioSource>().Play();
		
		// 局面を進める
		gameState.Advance(decidedMove);
		gameState.ShowBoard();
		
		Debug.Log("移動した駒:" + selectedPiece.ToString());
		
		isPieceSelected = false;
		selectedPiece.Outline.SetActive(false);
		selectedPiece = null;
		isBlackTurn = !isBlackTurn;

		// AIの手番の場合はAIの手を待つ
		if (!IsPlayerTurn())
		{
			await GetAIAction();
		}
	}

	/// <summary>
	/// 駒を動かす処理
	/// note: 持ち駒を動かす場合は必ずこちらの処理が呼ばれる
	/// </summary>
	/// <param name="cell"></param>
	/// <param name="isBlack"></param>
	private async UniTask MovePiece(Cell cell, bool isBlack)
	{
		if (!isPieceSelected)
		{
			return;
		}

		Move move = Move.NONE;
		Move movePromote = Move.NONE;
		Move decidedMove = Move.NONE;
		// 持ち駒を打つ場合
		if (selectedPiece.IsCaptured())
		{
			var pt = Converter.PieceTypeToDropPiece(selectedPiece.pieceType);
			var to = cell.SqPos(isAIFirst);
			move = Util.MakeMoveDrop(pt, to);
			decidedMove = move;
		}
		else
		{
			// 盤上の駒を移動する場合
			var from = selectedPiece.SqPos(isAIFirst);
			var to = cell.SqPos(isAIFirst);
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
				PromotePiece(selectedPiece, isAIFirst);
			}
			else
			{
				Debug.Log("不正な手です");
				isPieceSelected = false;
				selectedPiece.Outline.SetActive(false);
				selectedPiece = null;
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
				PromotePiece(selectedPiece, isAIFirst);
			}
			
			// 変数を初期化
			shouldPromote = false;
			promoteSelectionDone = false;
		}
		
		// 選択されている駒を移動させる
		selectedPiece.transform.SetParent(cell.transform);

		// 持ち駒が消費されていた場合は持ち駒情報を更新する
		if(selectedPiece.IsCaptured()) {
			capturePieceAreaData.UpdateCapturePieceData(selectedPiece.pieceType, isBlack, selectedPiece.IsCaptured());
            Debug.Log("持ち駒情報の更新 \n" + capturePieceAreaData);
            // 駒数表示を非有効化
            selectedPiece.IsActivePeiceNumText(0);

            var capturePieceArea = isBlack ? view.BlackCapturePieceArea : view.WhiteCapturePieceArea;

            // 最新のcapturePieceAreaDataを反映する前に古い持ち駒の表示を削除
			view.ClearCapturePieceArea(capturePieceArea.transform);

            // 駒台を更新
            foreach (PieceType pt in PieceData.getPieceTypeList(isBlack))
            {
                var pieceNum = capturePieceAreaData.getPieceNum(pt, isBlack);
                if (pieceNum == 0) continue;

                var pieceByPrefab = Instantiate(piecePrefab, capturePieceArea.transform);
                pieceByPrefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + PieceData.PieceTypeToStr(pt));
                pieceByPrefab.GetComponent<Piece>().piecePotition = new PieceData.PiecePotition(-1, -1);
                pieceByPrefab.GetComponent<Piece>().pieceType = pt;
                pieceByPrefab.GetComponent<Piece>().IsActivePeiceNumText(pieceNum);
                pieceByPrefab.GetComponent<Piece>().OnClickAction += UniTask.UnityAction(async () =>
                {
                    if (!IsPlayerTurn())
                    {
                        return;
                    }
                    await SelectPiece(pieceByPrefab.GetComponent<Piece>());
                });
            };

        };

		// 駒の位置を更新する
		selectedPiece.transform.localPosition = Vector3.zero;
		selectedPiece.piecePotition = new PieceData.PiecePotition(cell.x, cell.y);

        // 駒音を再生する
        selectedPiece.GetComponent<AudioSource>().Play();
		
		// 局面を進める
		gameState.Advance(decidedMove);
		gameState.ShowBoard();

		Debug.Log("移動した駒:" + selectedPiece.ToString());
		
		isPieceSelected = false;
		selectedPiece.Outline.SetActive(false);
		selectedPiece = null;
		isBlackTurn = !isBlackTurn;
		
		// AIの手番の場合はAIの手を待つ
		if (!IsPlayerTurn())
		{
			await GetAIAction();
		}
	}

	/// <summary>
	/// 駒を取る処理
	/// </summary>
	/// <param name="piece"></param>
	/// <param name="isBlack"></param>
	private void CapturePiece(Piece piece)
	{
		var capturePieceArea = IsPlayerTurn() ? view.BlackCapturePieceArea : view.WhiteCapturePieceArea;
		var pieceType = piece.pieceType;
		Debug.Log("取った駒:" + pieceType);

		capturePieceAreaData.UpdateCapturePieceData(pieceType, IsPlayerTurn());
        Debug.Log("持ち駒情報の更新 \n" + capturePieceAreaData);

		// 最新のcapturePieceAreaDataを反映する前に古い持ち駒の表示を削除
		view.ClearCapturePieceArea(capturePieceArea.transform);

        foreach (PieceType pt in PieceData.getPieceTypeList(IsPlayerTurn()))
        {
			var pieceNum = capturePieceAreaData.getPieceNum(pt, IsPlayerTurn());
			if(pieceNum == 0) continue;

            var pieceByPrefab = Instantiate(piecePrefab, capturePieceArea.transform);
            pieceByPrefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + PieceData.PieceTypeToStr(pt));
            pieceByPrefab.GetComponent<Piece>().piecePotition = new PieceData.PiecePotition(-1, -1);
            pieceByPrefab.GetComponent<Piece>().pieceType = pt;
            pieceByPrefab.GetComponent<Piece>().IsActivePeiceNumText(pieceNum);
            pieceByPrefab.GetComponent<Piece>().OnClickAction += UniTask.UnityAction(async () =>
            {
                if (!IsPlayerTurn())
                {
                    return;
                }
                await SelectPiece(pieceByPrefab.GetComponent<Piece>());
            });
        };

        // 取った駒を消去する
        Destroy(piece.gameObject);
	}
	
	private void PromotePiece(Piece piece, bool isAIFirst)
	{
		piece.isPromoted = true;
		piece.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + PieceData.PieceTypeToPromoteStr(piece.pieceType, isAIFirst));
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
	/// <param name="isBlack"></param>
	/// <returns></returns>
	private Piece GetCapturedPiece(PieceType pieceType, bool isBlack)
	{
		var capturePieceArea = isBlack ? view.BlackCapturePieceArea : view.WhiteCapturePieceArea;
		var piece = capturePieceArea.GetComponentInChildren<Piece>();
		return piece;
	}
	
	private bool IsPlayerTurn()
	{
		// 「先手のターンかつAIが後手」「後手のターンかつAIが先手」の場合、プレイヤーは手番を持っている
		return isBlackTurn != isAIFirst;
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
		isPieceSelected = true;

		if(isAIFirst)
		{
			fromX = 8 - fromX;
			fromY = 8 - fromY;
			toX = 8 - toX;
			toY = 8 - toY;
		}

		// 駒を打つ場合
		if (move.IsDrop())
		{
			var pieceType = Converter.DropPieceToPieceType(move.DroppedPiece(), isAIFirst);
			selectedPiece = GetCapturedPiece(pieceType, isAIFirst);
			await MovePiece(cells[toY, toX], isAIFirst);
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
		if (piece != null)
		{
			await SelectPiece(piece);
		}
		else
		{
			await MovePiece(cells[toY, toX], isAIFirst);
		}
		
		// 変数を初期化
		shouldPromote = false;
		promoteSelectionDone = false;
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