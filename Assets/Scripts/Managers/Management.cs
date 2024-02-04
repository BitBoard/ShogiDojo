using System;
using System.Collections;
using System.Collections.Generic;
using MyShogi.Model.Shogi.Core;
using UnityEngine;

public class Management : SingletonMonoBehaviour<Management>
{
	protected override void doAwake()
	{
		Init();
	}

	private void Init()
	{
		Initializer.Init(); // ゲームが始まる時に一回だけ呼ぶ
		Debug.Log("Management初期化完了");
	}
}