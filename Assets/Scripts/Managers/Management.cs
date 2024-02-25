using System;
using System.Collections;
using System.Collections.Generic;
using MyShogi.Model.Shogi.Core;
using UnityEngine;

public class Management : SingletonMonoBehaviour<Management>
{

	// 駒の画像を保存するDictionary
	private Dictionary<PieceType, Sprite> pieceSprites = new Dictionary<PieceType, Sprite>();

	protected override void doAwake()
	{
		Init();
	}

	private void Init()
	{
		Initializer.Init(); // ゲームが始まる時に一回だけ呼ぶ
		InitPieceSpritesDictionary(); // ゲーム開始時に一回だけ駒の画像を読み込む
        Debug.Log("Management初期化完了");
	}
	
	private void InitPieceSpritesDictionary() 
	{
		foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
		{
			pieceSprites[pieceType] = Resources.Load<Sprite>("ShogiUI/Piece/" + pieceType);
		}
	}

	public Sprite GetPieceSprite(PieceType pieceType)
	{
		return pieceSprites[pieceType];
	}
}