using System.Collections.Generic;

public class GameState
{
	public PieceType[,] board;
	public List<PieceType> capturedPieces; // 持ち駒
	public bool isCheck = false; // 王手がかかっているかどうか
	
	public GameState(BoardData boardData)
	{
		board = new PieceType[9,9];
		capturedPieces = new List<PieceType>();
		foreach (var data in boardData.boardData)
		{
			board[data.x, data.y] = PieceData.StrToPieceType(data.pieceType);
		}
	}

	public void Advance()
	{
		
	}
	
	public bool IsGameEnd()
	{
		return false;
	}
	
}