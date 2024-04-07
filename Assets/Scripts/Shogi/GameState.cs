using System.Collections.Generic;
using MyShogi.Model.Shogi.Core;
using UnityEngine;
using Color = MyShogi.Model.Shogi.Core.Color;

/// <summary>
/// ゲームの状態を表すクラス
/// </summary>
public class GameState
{
	private Position position;

	public GameState(BoardType boardType = BoardType.NoHandicap)
	{
		position = new Position();
		position.InitBoard(boardType);
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
	/// ある手が合法手かどうかを判定する
	/// </summary>
	/// <param name="move"></param>
	/// <returns></returns>
	public bool IsValidMove(Move move)
	{
		return position.IsLegal(move);
	}
	
	/// <summary>
	/// 現在の手番のプレイヤーを返す
	/// </summary>
	public Color GetTurnPlayer()
	{
		return position.sideToMove;
	}
	
	/// <summary>
	/// 現局面の盤面情報を表示する
	/// </summary>
	public void ShowBoard()
	{
		Debug.Log(position.Pretty());
	}

	/// <summary>
	/// 現在の局面の指定したプレイヤーから見た駒割のスコアを返す
	/// </summary>
	public int GetPieceScore(Color color)
	{
		// 駒割のスコアを計算する
		var blackScore = position.GetTotalPieceScore(Color.BLACK);
		var whiteScore = position.GetTotalPieceScore(Color.WHITE);
		
		var score = color == Color.BLACK ? blackScore - whiteScore : whiteScore - blackScore;

		return score;
	}
	
	/// <summary>
	/// 探索時のオブジェクト複製用メソッド
	/// </summary>
	public GameState Clone()
	{
		var clone = new GameState
		{
			position = position.Clone()
		};
		return clone;
	}
	
}