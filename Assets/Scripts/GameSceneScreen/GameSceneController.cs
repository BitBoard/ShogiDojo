using System;
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
            cell.OnClickAction += () =>
			{
				if (selectedPiece != null)
				{
					MovePiece(cell);
				}
			};
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
			piece.GetComponent<Piece>().OnClickAction += () =>
			{
				SelectPiece(piece.GetComponent<Piece>());
			};
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
	
	private void SelectPiece(Piece piece)
	{
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
		Debug.Log("指し手:" + move.Pretty());
		if (!gameState.IsValidMove(move))
		{
			Debug.Log("不正な手です");
			isPieceSelected = false;
			selectedPiece.Outline.SetActive(false);
			selectedPiece = null;
			return;
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
		gameState.Advance(move);
		gameState.ShowBoard();
		
		Debug.Log("移動した駒:" + selectedPiece.ToString());
		
		isPieceSelected = false;
		selectedPiece.Outline.SetActive(false);
		selectedPiece = null;
		isBlackTurn = !isBlackTurn;
	}

	private void MovePiece(Cell cell)
	{
		if (!isPieceSelected)
		{
			return;
		}

		Move move;
		if (selectedPiece.IsCaptured())
		{
			var pt = Converter.PieceTypeToPiece(selectedPiece.pieceType, selectedPiece.isPromoted);
			var to = cell.SqPos;
			move = Util.MakeMoveDrop(pt, to);
		}
		else
		{
			var from = selectedPiece.SqPos;
			var to = cell.SqPos;
			move = Util.MakeMove(from, to);
		}

		// 合法手かどうかを判定する
		Debug.Log("指し手:" + move.Pretty());
		if (!gameState.IsValidMove(move))
		{
			Debug.Log("不正な手です");
			isPieceSelected = false;
			selectedPiece.Outline.SetActive(false);
			selectedPiece = null;
			return;
		}
		
		// 選択されている駒を移動させる
		selectedPiece.transform.SetParent(cell.transform);
		// 駒の位置を更新する
		selectedPiece.transform.localPosition = Vector3.zero;
		selectedPiece.piecePotition = new PieceData.PiecePotition(cell.x, cell.y);

		// 駒音を再生する
		selectedPiece.GetComponent<AudioSource>().Play();
		
		// 局面を進める
		gameState.Advance(move);
		gameState.ShowBoard();

		Debug.Log("移動した駒:" + selectedPiece.ToString());
		
		isPieceSelected = false;
		selectedPiece.Outline.SetActive(false);
		selectedPiece = null;
		isBlackTurn = !isBlackTurn;
	}

	private void CapturePiece(Piece piece, bool isBlack)
	{
		var capturePieceArea = isBlack ? view.BlackCapturePieceArea : view.WhiteCapturePieceArea;
		var pieceType = piece.pieceType;
		Debug.Log("取った駒:" + pieceType);
		var capturePiece = Instantiate(piecePrefab, capturePieceArea.transform);
		capturePiece.GetComponent<Image>().sprite = Resources.Load<Sprite>("ShogiUI/Piece/" + PieceData.CapturePieceTypeToStr(pieceType));
		capturePiece.GetComponent<Piece>().pieceType = PieceData.GetCapturePieceType(pieceType);
		capturePiece.GetComponent<Piece>().piecePotition = new PieceData.PiecePotition(-1, -1);
		capturePiece.GetComponent<Piece>().OnClickAction += () =>
		{
			SelectPiece(capturePiece.GetComponent<Piece>());
		};
		// 取った駒を消去する
		Destroy(piece.gameObject);
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