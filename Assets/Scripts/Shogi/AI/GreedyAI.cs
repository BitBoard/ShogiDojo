using MyShogi.Model.Shogi.Core;
using UnityEngine;
using Color = MyShogi.Model.Shogi.Core.Color;

/// <summary>
/// 一手先の駒得狙いのAI
/// </summary>
public class GreedyAI : IShogiAI
{
	public Move GetMove(GameState gameState)
	{
		var moves = gameState.GetLegalMoves();
		Debug.Log("合法手の数:" + moves.Count);
		
		// 現在の手番の色
		var color = gameState.GetTurnPlayer();
		
		// 合法手の中で最も評価値が高い手を選ぶ
		var bestMove = moves[0];
		var bestEval = -100000;
		foreach (var move in moves)
		{
			var nextGameState = gameState.Clone();
			nextGameState.Advance(move);
			var eval = nextGameState.GetPieceScore(color);
			
			if (nextGameState.IsMated())
			{
				eval = 999999;
			}

			if (eval > bestEval)
			{
				bestEval = eval;
				bestMove = move;
			}
		}
		
		Debug.Log("AI側から見た評価値:" + bestEval);
		
		return bestMove;
	}
	
}