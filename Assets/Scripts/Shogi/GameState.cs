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
	public List<Move> GetLegalMoves()
	{
		var moves = new Move[(int)Move.MAX_MOVES];
		var endIndex = MoveGen.LegalAll(position, moves, 0);
		
		// endIndexまでの指し手が合法手
		var legalMoves = new List<Move>();
		for (var i = 0; i < endIndex; i++)
		{
			legalMoves.Add(moves[i]);
		}
		
		return legalMoves;
	}

	/// <summary>
	/// 指定手を着手し局面を進める
	/// </summary>
	public void Advance(Move move)
	{
		position.DoMove(move);
	}

	/// <summary>
	/// 現局面で手番のプレイヤーが詰んでいるかどうかを判定する
	/// </summary>
	/// <returns></returns>
	public bool IsMated()
	{
		var moves = new Move[(int)Move.MAX_MOVES];
		return position.IsMated(moves);
	}
	
	/// <summary>
	/// 現局面の盤面情報を表示する
	/// </summary>
	public void ShowBoard()
	{
		Debug.Log(position.Pretty());
	}
	
}