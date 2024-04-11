using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyShogi.Model.Shogi.Core;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AlphaBetaAI : IShogiAI
{
	private const int _timeLimit = 10000; 
	
	public async UniTask<Move> GetMove(GameState gameState, CancellationToken token = default)
	{
		var moves = gameState.GetLegalMoves();
		Debug.Log("合法手の数:" + moves.Count);

		// 合法手の中で最も評価値が高い手を選ぶ
		var bestMove = moves[0];
		int alpha = -10000000;
		int beta = 10000000;

		// 時間計測
		var sw = new Stopwatch();
		sw.Start();

		// 反復深化
		for (int depth = 1;; depth++)
		{
			foreach (var move in moves)
			{
			
#if UNITY_WEBGL
				if (sw.ElapsedMilliseconds % 1000 > 0)
				{
					await UniTask.DelayFrame(1, cancellationToken: token);
				}
#endif
				// 指定秒以上かかったら打ち切る
				if (sw.ElapsedMilliseconds > _timeLimit)
				{
					sw.Stop();
					Debug.Log($"探索タイムアウト{_timeLimit / 1000}秒");
					Debug.Log("AI側から見た評価値:" + alpha);
					return bestMove;
				}
			
				var nextGameState = gameState.Clone();
				nextGameState.Advance(move);
			
				if (nextGameState.IsMated())
				{
					Debug.Log("詰みを発見しました");
					return move;
				}

#if UNITY_WEBGL
				// WebGLではマルチスレッド処理を使わない
				var score = - await AlphaBetaSearch(nextGameState, depth, -beta, -alpha, sw, token);
#else
				// 相手から見た評価値になるので反転させる
				var score = - await UniTask.RunOnThreadPool(() => AlphaBetaSearch(nextGameState, depth, -beta, -alpha, sw, token), cancellationToken: token); 
#endif
				if (score > alpha)
				{
					alpha = score;
					bestMove = move;
				}
			}
		}
	}
	
	private async UniTask<int> AlphaBetaSearch(GameState gameState, int depth, int alpha, int beta, Stopwatch sw, CancellationToken token = default)
	{
		// 詰んでいるか、探索深さが0になったら評価値を返す
		if (gameState.IsMated() || depth == 0)
		{
			return gameState.GetPieceScore();
		}

#if UNITY_WEBGL
		if (sw.ElapsedMilliseconds % 1000 > 0)
		{
			await UniTask.DelayFrame(1, cancellationToken: token);
		}
#endif
		
		// 探索が指定秒以上かかったら打ち切る
		if (sw.ElapsedMilliseconds > _timeLimit)
		{
			sw.Stop();
			return gameState.GetPieceScore();
		}
		
		var moves = gameState.GetLegalMoves();

		foreach (var move in moves)
		{
			var nextGameState = gameState.Clone();
			nextGameState.Advance(move);
#if UNITY_WEBGL
			// WebGLではマルチスレッド処理を使えない
			var score = - await AlphaBetaSearch(nextGameState, depth - 1, -beta, -alpha, sw, token);
#else
			var score = - await UniTask.RunOnThreadPool(() => AlphaBetaSearch(nextGameState, depth - 1, -beta, -alpha, sw), cancellationToken: token);
#endif			
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
	