using System.Collections.Generic;

/// <summary>
/// ゲームの状態を表すクラス
/// </summary>
public class GameState
{
	public PieceType[,] board; // 盤面
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
	
	/// <summary>
	/// 現在の手番プレイヤーの合法手を返す
	/// </summary>
	/// <returns></returns>
	public List<string> GetLegalMoves()
	{
		return new List<string>();
	}

	/// <summary>
	/// 指定手を着手し局面を進める
	/// </summary>
	public void Advance()
	{
		
	}
	
	/// <summary>
	/// ゲームが終了しているかどうかを判定する
	/// </summary>
	/// <returns></returns>
	public bool IsGameEnd()
	{
		return false;
	}
	
}