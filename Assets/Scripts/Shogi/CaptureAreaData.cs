﻿using System;
using MyShogi.Model.Shogi.Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;

public interface ICapturePieceNum
{
    int pawn { get; set; }
    int lance { get; set; }
    int knight { get; set; }
    int silver { get; set; }
    int gold { get; set; }
    int bishop { get; set; }
    int rook { get; set; }
    int king { get; set; }
}

// 先手後手の持ち駒に関する情報を管理するクラス
public class CapturePieceAreaData 
{
    public ICapturePieceNum blackCapturePiece;
    public ICapturePieceNum whiteCapturePiece;

    public override string ToString()
    {
        return $"Black: Pawn  , Num: {blackCapturePiece.pawn} \n" +
               $"Black: Lance , Num: {blackCapturePiece.lance} \n" +
               $"Black: Knight, Num: {blackCapturePiece.knight} \n" +
               $"Black: Silver, Num: {blackCapturePiece.silver} \n" +
               $"Black: Gold  , Num: {blackCapturePiece.gold} \n" +
               $"Black: Bishop, Num: {blackCapturePiece.bishop} \n" +
               $"Black: Rook  , Num: {blackCapturePiece.rook} \n" +
               $"Black: King  , Num: {blackCapturePiece.king} \n" +
               $"White: Pawn  , Num: {whiteCapturePiece.pawn} \n" +
               $"White: Lance , Num: {whiteCapturePiece.lance} \n" +
               $"White: Knight, Num: {whiteCapturePiece.knight} \n" +
               $"White: Silver, Num: {whiteCapturePiece.silver} \n" +
               $"White: Gold  , Num: {whiteCapturePiece.gold} \n" +
               $"White: Bishop, Num: {whiteCapturePiece.bishop} \n" +
               $"White: Rook  , Num: {whiteCapturePiece.rook} \n" +
               $"White: King  , Num: {whiteCapturePiece.king} ";               ;
    }

    public CapturePieceAreaData()
    {
        InitCaptureArea();
    }

    private void InitCaptureArea()
    {
        blackCapturePiece = CreateEmptyCapturePiece();
        whiteCapturePiece = CreateEmptyCapturePiece();
        Debug.Log("持ち駒情報の初期化が完了");
    }

    public int getPieceNum(PieceType pieceType, bool isBlack)
    {
        ICapturePieceNum capturePiece = isBlack ? blackCapturePiece : whiteCapturePiece;

        switch (pieceType)
        {
            case PieceType.BlackPawn:
            case PieceType.WhitePawn:
                return capturePiece.pawn;
            case PieceType.BlackLance:
            case PieceType.WhiteLance:
                return capturePiece.lance;
            case PieceType.BlackKnight:
            case PieceType.WhiteKnight:
                return capturePiece.knight;
            case PieceType.BlackSilver:
            case PieceType.WhiteSilver:
                return capturePiece.silver;
            case PieceType.BlackGold:
            case PieceType.WhiteGold:
                return capturePiece.gold;
            case PieceType.BlackBishop:
            case PieceType.WhiteBishop:
                return capturePiece.bishop;
            case PieceType.BlackRook:
            case PieceType.WhiteRook:
                return capturePiece.rook;
            case PieceType.BlackKing:
            case PieceType.WhiteKing:
                return capturePiece.king;
            default:
                Debug.LogError("Unknown pieceType: " + pieceType);
                return 0;
        }
    }
    
    public void UpdateCapturePieceData(PieceType pieceType, bool isBlack, bool isCaptured = false)
    {
        ICapturePieceNum capturePiece = isBlack ? blackCapturePiece : whiteCapturePiece;

        // 持ち駒を消費した場合と取得し場合で更新式を分岐
        switch (pieceType)
        {
            case PieceType.BlackPawn:
            case PieceType.WhitePawn:
                capturePiece.pawn += isCaptured ? -1 : 1;
                break;
            case PieceType.BlackLance:
            case PieceType.WhiteLance:
                capturePiece.lance += isCaptured ? -1 : 1;
                break;
            case PieceType.BlackKnight:
            case PieceType.WhiteKnight:
                capturePiece.knight += isCaptured ? -1 : 1;
                break;
            case PieceType.BlackSilver:
            case PieceType.WhiteSilver:
                capturePiece.silver += isCaptured ? -1 : 1;
                break;
            case PieceType.BlackGold:
            case PieceType.WhiteGold:
                capturePiece.gold += isCaptured ? -1 : 1;
                break;
            case PieceType.BlackBishop:
            case PieceType.WhiteBishop:
                capturePiece.bishop += isCaptured ? -1 : 1;
                break;
            case PieceType.BlackRook:
            case PieceType.WhiteRook:
                capturePiece.rook += isCaptured ? -1 : 1;
                break;
            case PieceType.BlackKing:
            case PieceType.WhiteKing:
                capturePiece.king += isCaptured ? -1 : 1;
                break;
            default:
                Debug.LogError("Unknown pieceType: " + pieceType);
                break;
        }
    }

    private ICapturePieceNum CreateEmptyCapturePiece()
    {
        return new CapturePiece
        {
            pawn = 0,
            lance = 0,
            knight = 0,
            silver = 0,
            gold = 0,
            bishop = 0,
            rook = 0,
            king = 0
        };
    }

}
public class CapturePiece : ICapturePieceNum
{
    public int pawn { get; set; }
    public int lance { get; set; }
    public int knight { get; set; }
    public int silver { get; set; }
    public int gold { get; set; }
    public int bishop { get; set; }
    public int rook { get; set; }
    public int king { get; set; }
}