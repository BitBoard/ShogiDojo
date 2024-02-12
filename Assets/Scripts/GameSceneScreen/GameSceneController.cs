using System;
using Cysharp.Threading.Tasks;
using MyShogi.Model.Shogi.Core;
using UnityEngine;
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
    private bool isBlackTurn = true;
    private bool shouldPromote = false;
    private bool promoteSelectionDone = false;
	private CapturePieceAreaData capturePieceAreaData;

    private void Awake()
	{
		Init();
	}
	
	private void Start()
	{
		gameState = new GameState();
		gameState.ShowBoard();
	}

	private void Init()
	{
        SetEvent();
        SetCells();
		InitBoard();
	}
	
	private void SetEvent()
	{
		view.OpenDebugMenuButton.onClick.AddListener(OpenDebugMenu);
		view.CloseDebugMenuButton.onClick.AddListener(CloseDebugMenu);
		configPopupController.action += InitBoard;
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

	private void InitBoard(string boardJsonPath = "", BoardType boardType = BoardType.NoHandicap)
    {
        ClearPieces();
        capturePieceAreaData = new CapturePieceAreaData();
        isPieceSelected = false;
        selectedPiece = null;
		isBlackTurn = true;
        gameState = new GameState(boardType);
        gameState.ShowBoard();

        if (String.IsNullOrEmpty(boardJsonPath))
		{
			boardJsonPath = GameConfig.initialBoardJsonPath;
        }
		var json = Resources.Load<TextAsset>(boardJsonPath).ToString();
        var boardData = JsonUtility.FromJson<BoardData>(json);

		Debug.Log(boardData);
        foreach (var data in boardData.boardData)
        {
            var piece = Instantiate(piecePrefab, cells[data.y, data.x].transform);
            piece.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + data.pieceType);
            piece.GetComponent<Piece>().pieceType = PieceData.StrToPieceType(data.pieceType);
			piece.GetComponent<Piece>().piecePotition = new PieceData.PiecePotition(data.x, data.y);
			piece.GetComponent<Piece>().OnClickAction += UniTask.UnityAction(async () =>
			{
				await SelectPiece(piece.GetComponent<Piece>());
			});
			
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
		
		var from = selectedPiece.SqPos;
		var to = piece.SqPos;
		
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
				PromotePiece(selectedPiece);
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
			view.PromotePopupView.gameObject.SetActive(true);
			shouldPromote = false;
			promoteSelectionDone = false;
			
			// ここで待機する
			await UniTask.WaitUntil(() => promoteSelectionDone);
			
			if (shouldPromote)
			{
				decidedMove = movePromote;
				PromotePiece(selectedPiece);
			}
		}
		

		// 選択されている駒を移動させる
		selectedPiece.transform.SetParent(piece.transform.parent);
		// 移動先のマスの駒を取る
		CapturePiece(piece, isBlackTurn);
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
			var to = cell.SqPos;
			move = Util.MakeMoveDrop(pt, to);
			decidedMove = move;
		}
		else
		{
			// 盤上の駒を移動する場合
			var from = selectedPiece.SqPos;
			var to = cell.SqPos;
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
				PromotePiece(selectedPiece);
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
			view.PromotePopupView.gameObject.SetActive(true);
			shouldPromote = false;
			promoteSelectionDone = false;
			
			// ここで待機する
			await UniTask.WaitUntil(() => promoteSelectionDone);
			
			if (shouldPromote)
			{
				decidedMove = movePromote;
				PromotePiece(selectedPiece);
			}
		}
		
		// 選択されている駒を移動させる
		selectedPiece.transform.SetParent(cell.transform);

		// 持ち駒が消費されていた場合は持ち駒情報を更新する
		if(selectedPiece.IsCaptured()) {
			capturePieceAreaData.UpdateCapturePieceData(selectedPiece.pieceType, isBlack, selectedPiece.IsCaptured());
            Debug.Log("持ち駒情報の更新 \n" + capturePieceAreaData);
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
	}

	/// <summary>
	/// 駒を取る処理
	/// </summary>
	/// <param name="piece"></param>
	/// <param name="isBlack"></param>
	private void CapturePiece(Piece piece, bool isBlack)
	{
		var capturePieceArea = isBlack ? view.BlackCapturePieceArea : view.WhiteCapturePieceArea;
		var pieceType = piece.pieceType;
		Debug.Log("取った駒:" + pieceType);

		capturePieceAreaData.UpdateCapturePieceData(pieceType, isBlack);
        Debug.Log("持ち駒情報の更新 \n" + capturePieceAreaData);

        // 最新のcapturePieceAreaDataを反映する前に古い持ち駒の表示を削除
        foreach (Transform child in capturePieceArea.transform)
        {
            Destroy(child.gameObject);
        }

        // capturePieceAreaDataの情報を元に持ち駒の表示を更新
        foreach (PieceType pt in PieceData.getPieceTypeList(isBlack)) {
			for (var pieceNum = 0; pieceNum < capturePieceAreaData.getPieceNum(pt, isBlack); pieceNum++)
			{
				var pieceByPrefab = Instantiate(piecePrefab, capturePieceArea.transform);
                pieceByPrefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + PieceData.PieceTypeToStr(pt));
                pieceByPrefab.GetComponent<Piece>().piecePotition = new PieceData.PiecePotition(-1, -1);
                pieceByPrefab.GetComponent<Piece>().pieceType = pt;
                pieceByPrefab.GetComponent<Piece>().OnClickAction += UniTask.UnityAction(async () =>
                {
	                await SelectPiece(pieceByPrefab.GetComponent<Piece>());
                });
			}
		};

        // 取った駒を消去する
        Destroy(piece.gameObject);
	}
	
	private void PromotePiece(Piece piece)
	{
		piece.isPromoted = true;
		piece.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + PieceData.PieceTypeToPromoteStr(piece.pieceType));
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