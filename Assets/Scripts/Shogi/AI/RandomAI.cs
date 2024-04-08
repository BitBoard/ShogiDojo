using Cysharp.Threading.Tasks;
using MyShogi.Model.Shogi.Core;
using UnityEngine;

public class RandomAI : IShogiAI
{
	public async UniTask<Move> GetMove(GameState gameState)
	{
		await UniTask.Delay(1000);
		
		var moves = gameState.GetLegalMoves();
		Debug.Log("合法手の数:" + moves.Count);
		var randomIndex = Random.Range(0, moves.Count);
		Debug.Log("ランダムなインデックス:" + randomIndex);
		return moves[randomIndex];
	}
}