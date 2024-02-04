using MyShogi.Model.Shogi.Core;
using UnityEngine;

public class RandomAI : IShogiAI
{
	public Move GetMove(GameState gameState)
	{
		var moves = gameState.GetLegalMoves();
		Debug.Log("合法手の数:" + moves.Count);
		var randomIndex = Random.Range(0, moves.Count);
		Debug.Log("ランダムなインデックス:" + randomIndex);
		return moves[randomIndex];
	}
}