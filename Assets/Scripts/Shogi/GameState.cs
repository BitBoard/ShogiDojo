using System.Collections.Generic;
using MyShogi.Model.Shogi.Core;
using UnityEngine;

/// <summary>
/// ゲームの状態を表すクラス
/// </summary>
public class GameState
{

	public GameState()
	{
		Initializer.Init();
		var position = new Position();
		position.InitBoard();
		Debug.Log(position.Pretty());
	}
	
	/// <summary>
	/// 現在の手番プレイヤーの合法手を返す
	/// </summary>
	/// <returns></returns>
	public List<string> GetLegalMoves()
	{
		return new List<string>();
	}

	/// <summary>
	/// 指定手を着手し局面を進める
	/// </summary>
	public void Advance()
	{
		
	}
	
	/// <summary>
	/// ゲームが終了しているかどうかを判定する
	/// </summary>
	/// <returns></returns>
	public bool IsGameEnd()
	{
		return false;
	}
	
}