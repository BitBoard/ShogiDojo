using System.Threading;
using Cysharp.Threading.Tasks;
using MyShogi.Model.Shogi.Core;

public interface IShogiAI
{ 
	UniTask<Move> GetMove(GameState gameState, CancellationToken token);
}