using MyShogi.Model.Shogi.Core;

public interface IShogiAI
{ 
	Move GetMove(GameState gameState);
}