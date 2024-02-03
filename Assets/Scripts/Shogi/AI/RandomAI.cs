using MyShogi.Model.Shogi.Core;
using UnityEngine;

public class RandomAI : IShogiAI
{
	public Move GetMove(GameState gameState)
	{
		var moves = gameState.GetLegalMoves();
		var randomIndex = Random.Range(0, moves.Length);
		return moves[randomIndex];
	}
}