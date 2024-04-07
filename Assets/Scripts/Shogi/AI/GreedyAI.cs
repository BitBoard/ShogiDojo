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
		var bestEval = -10000000;
		foreach (var move in moves)
		{
			var nextGameState = gameState.Clone();
			nextGameState.Advance(move);
			// 相手から見た評価値になるので反転させる
			var score = - nextGameState.GetPieceScore();
			
			if (nextGameState.IsMated())
			{
				Debug.Log("詰みを発見しました");
				return move;
			}

			if (score > bestEval)
			{
				bestEval = score;
				bestMove = move;
			}
		}
		
		Debug.Log("AI側から見た評価値:" + bestEval);
		
		return bestMove;
	}
	
}