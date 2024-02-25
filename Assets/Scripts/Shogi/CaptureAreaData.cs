using System;
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
    private ICapturePieceNum frontCapturePiece;
    private ICapturePieceNum backCapturePiece;

    public override string ToString()
    {
        return $"Front: Pawn  , Num: {frontCapturePiece.pawn} \n" +
               $"Front: Lance , Num: {frontCapturePiece.lance} \n" +
               $"Front: Knight, Num: {frontCapturePiece.knight} \n" +
               $"Front: Silver, Num: {frontCapturePiece.silver} \n" +
               $"Front: Gold  , Num: {frontCapturePiece.gold} \n" +
               $"Front: Bishop, Num: {frontCapturePiece.bishop} \n" +
               $"Front: Rook  , Num: {frontCapturePiece.rook} \n" +
               $"Front: King  , Num: {frontCapturePiece.king} \n" +
               $"Back: Pawn  , Num: {backCapturePiece.pawn} \n" +
               $"Back: Lance , Num: {backCapturePiece.lance} \n" +
               $"Back: Knight, Num: {backCapturePiece.knight} \n" +
               $"Back: Silver, Num: {backCapturePiece.silver} \n" +
               $"Back: Gold  , Num: {backCapturePiece.gold} \n" +
               $"Back: Bishop, Num: {backCapturePiece.bishop} \n" +
               $"Back: Rook  , Num: {backCapturePiece.rook} \n" +
               $"Back: King  , Num: {backCapturePiece.king} ";               ;
    }

    public CapturePieceAreaData()
    {
        InitCaptureArea();
    }

    private void InitCaptureArea()
    {
        frontCapturePiece = CreateEmptyCapturePiece();
        backCapturePiece = CreateEmptyCapturePiece();
        Debug.Log("持ち駒情報の初期化が完了");
    }

    public int getPieceNum(PieceType pieceType, bool isFront)
    {
        ICapturePieceNum capturePiece = isFront ? frontCapturePiece : backCapturePiece;

        switch (pieceType)
        {
            case PieceType.FrontPawn:
            case PieceType.BackPawn:
                return capturePiece.pawn;
            case PieceType.FrontLance:
            case PieceType.BackLance:
                return capturePiece.lance;
            case PieceType.FrontKnight:
            case PieceType.BackKnight:
                return capturePiece.knight;
            case PieceType.FrontSilver:
            case PieceType.BackSilver:
                return capturePiece.silver;
            case PieceType.FrontGold:
            case PieceType.BackGold:
                return capturePiece.gold;
            case PieceType.FrontBishop:
            case PieceType.BackBishop:
                return capturePiece.bishop;
            case PieceType.FrontRook:
            case PieceType.BackRook:
                return capturePiece.rook;
            case PieceType.FrontKing:
            case PieceType.BackKing:
                return capturePiece.king;
            default:
                Debug.LogError("Unknown pieceType: " + pieceType);
                return 0;
        }
    }
    
    public void UpdateCapturePieceData(PieceType pieceType, bool isFront, bool isCaptured = false)
    {
        ICapturePieceNum capturePiece = isFront ? frontCapturePiece : backCapturePiece;

        // 持ち駒を消費した場合と取得し場合で更新式を分岐
        switch (pieceType)
        {
            case PieceType.FrontPawn:
            case PieceType.BackPawn:
            case PieceType.FrontPawnPromoted:
            case PieceType.BackPawnPromoted:
                capturePiece.pawn += isCaptured ? -1 : 1;
                break;
            case PieceType.FrontLance:
            case PieceType.BackLance:
            case PieceType.FrontLancePromoted:
            case PieceType.BackLancePromoted:
                capturePiece.lance += isCaptured ? -1 : 1;
                break;
            case PieceType.FrontKnight:
            case PieceType.BackKnight:
            case PieceType.FrontKnightPromoted:
            case PieceType.BackKnightPromoted:
                capturePiece.knight += isCaptured ? -1 : 1;
                break;
            case PieceType.FrontSilver:
            case PieceType.BackSilver:
            case PieceType.FrontSilverPromoted:
            case PieceType.BackSilverPromoted:
                capturePiece.silver += isCaptured ? -1 : 1;
                break;
            case PieceType.FrontGold:
            case PieceType.BackGold:
                capturePiece.gold += isCaptured ? -1 : 1;
                break;
            case PieceType.FrontBishop:
            case PieceType.BackBishop:
            case PieceType.FrontBishopPromoted:
            case PieceType.BackBishopPromoted:
                capturePiece.bishop += isCaptured ? -1 : 1;
                break;
            case PieceType.FrontRook:
            case PieceType.BackRook:
            case PieceType.FrontRookPromoted:
            case PieceType.BackRookPromoted:
                capturePiece.rook += isCaptured ? -1 : 1;
                break;
            case PieceType.FrontKing:
            case PieceType.BackKing:
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