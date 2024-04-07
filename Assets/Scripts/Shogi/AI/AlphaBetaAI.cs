using MyShogi.Model.Shogi.Core;
using UnityEngine;
using Color = MyShogi.Model.Shogi.Core.Color;

public class AlphaBetaAI : IShogiAI
{
	public Move GetMove(GameState gameState)
	{
		var moves = gameState.GetLegalMoves();
		Debug.Log("合法手の数:" + moves.Count);
		
		// 現在の手番の色
		var color = gameState.GetTurnPlayer();
		
		// 合法手の中で最も評価値が高い手を選ぶ
		var bestMove = moves[0];
		int alpha = -10000000;
		int beta = 10000000;

		// 時間計測
		var sw = new System.Diagnostics.Stopwatch();
		sw.Start();

		foreach (var move in moves)
		{
			// 5秒以上かかったら打ち切る
			if (sw.ElapsedMilliseconds > 5000)
			{
				Debug.Log("探索タイムアウト（5秒）");
				return bestMove;
			}
			
			var nextGameState = gameState.Clone();
			nextGameState.Advance(move);
			
			if (nextGameState.IsMated())
			{
				Debug.Log("詰みを発見しました");
				return move;
			}
			
			// 相手から見た評価値になるので反転させる
			var score = - AlphaBetaSearch(nextGameState, 1, alpha, beta);
			
			if (score > alpha)
			{
				alpha = score;
				bestMove = move;
			}
		}
		
		Debug.Log("探索完了");
		Debug.Log("AI側から見た評価値:" + alpha);
		
		return bestMove;
	}
	
	private int AlphaBetaSearch(GameState gameState, int depth, int alpha, int beta)
	{
		// 詰んでいるか、探索深さが0になったら評価値を返す
		if (gameState.IsMated() || depth == 0)
		{
			return gameState.GetPieceScore();
		}
		
		var moves = gameState.GetLegalMoves();

		foreach (var move in moves)
		{
			var nextGameState = gameState.Clone();
			nextGameState.Advance(move);
			var score = - AlphaBetaSearch(nextGameState, depth - 1, alpha, beta);
			
			if (score > alpha)
			{
				alpha = score;
			}
			
			if (alpha >= beta)
			{
				return alpha;
			}
		}
		
		return alpha;
		
	}
		
}
	