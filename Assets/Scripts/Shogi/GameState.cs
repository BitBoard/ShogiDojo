using System.Collections.Generic;
using MyShogi.Model.Shogi.Core;
using UnityEngine;

/// <summary>
/// ゲームの状態を表すクラス
/// </summary>
public class GameState
{
	private Position position;

	public GameState()
	{
		position = new Position();
		position.InitBoard();
	}
	
	/// <summary>
	/// 現在の手番プレイヤーの合法手を返す
	/// </summary>
	/// <returns></returns>
	public Move[] GetLegalMoves()
	{
		var moves = new Move[(int)Move.MAX_MOVES];
		var _ = MoveGen.LegalAll(position, moves, 0);

		return moves;
	}

	/// <summary>
	/// 指定手を着手し局面を進める
	/// </summary>
	public void Advance(Move move)
	{
		position.DoMove(move);
	}
	
	/// <summary>
	/// ゲームが終了しているかどうかを判定する
	/// </summary>
	/// <returns></returns>
	public bool IsGameEnd()
	{
		return false;
	}
	
	public void ShowBoard()
	{
		Debug.Log(position.Pretty());
	}
	
}