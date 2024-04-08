using System.Diagnostics;
using Cysharp.Threading.Tasks;
using MyShogi.Model.Shogi.Core;
using Debug = UnityEngine.Debug;

public class AlphaBetaAI : IShogiAI
{
	public async UniTask<Move> GetMove(GameState gameState)
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

		foreach (var move in moves)
		{
			// 10秒以上かかったら打ち切る
			if (sw.ElapsedMilliseconds > 10000)
			{
				sw.Stop();
				Debug.Log("探索タイムアウト（10秒）");
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
			
			// 相手から見た評価値になるので反転させる
			var score = - await UniTask.RunOnThreadPool(() => AlphaBetaSearch(nextGameState, 2, -beta, -alpha, sw)); 
			
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
	
	private async UniTask<int> AlphaBetaSearch(GameState gameState, int depth, int alpha, int beta, Stopwatch sw)
	{
		// 詰んでいるか、探索深さが0になったら評価値を返す
		if (gameState.IsMated() || depth == 0)
		{
			return gameState.GetPieceScore();
		}
		
		// 探索が10秒以上かかったら打ち切る
		if (sw.ElapsedMilliseconds > 10000)
		{
			sw.Stop();
			return gameState.GetPieceScore();
		}
		
		var moves = gameState.GetLegalMoves();

		foreach (var move in moves)
		{
			var nextGameState = gameState.Clone();
			nextGameState.Advance(move);
			var score = - await UniTask.RunOnThreadPool(() => AlphaBetaSearch(nextGameState, depth - 1, -beta, -alpha, sw));
			
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
	