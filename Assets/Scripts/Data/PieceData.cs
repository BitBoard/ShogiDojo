using System;
using System.Collections;
using UnityEngine;

public class PieceData {

    public static PieceType StrToPieceType(string pieceTypeStr)
    {
        // ここで文字列からswitch文でPieceTypeに変換する
        // 例: "white_pawn" -> PieceType.WhitePawn
        switch (pieceTypeStr)
        {
            case "BackPawn":
                return PieceType.BackPawn;
            case "BackLance":
                return PieceType.BackLance;
            case "BackKnight":
                return PieceType.BackKnight;
            case "BackSilver":
                return PieceType.BackSilver;
            case "BackGold":
                return PieceType.BackGold;
            case "BackBishop":
                return PieceType.BackBishop;
            case "BackRook":
                return PieceType.BackRook;
            case "BackKing":
                return PieceType.BackKing;
            case "FrontPawn":
                return PieceType.FrontPawn;
            case "FrontLance":
                return PieceType.FrontLance;
            case "FrontKnight":
                return PieceType.FrontKnight;
            case "FrontSilver":
                return PieceType.FrontSilver;
            case "FrontGold":
                return PieceType.FrontGold;
            case "FrontBishop":
                return PieceType.FrontBishop;
            case "FrontRook":
                return PieceType.FrontRook;
            case "FrontKing":
                return PieceType.FrontKing;
            default:
                return PieceType.None;
        }
    }

    public static PieceType[] GetPieceTypeList(bool isBlack)
    {
        return isBlack ?
            new PieceType[]
            {
                PieceType.FrontPawn,
                PieceType.FrontLance,
                PieceType.FrontKnight,
                PieceType.FrontSilver,
                PieceType.FrontGold,
                PieceType.FrontBishop,
                PieceType.FrontRook,
                PieceType.FrontKing,
            } :
            new PieceType[]
            {
                PieceType.BackPawn,
                PieceType.BackLance,
                PieceType.BackKnight,
                PieceType.BackSilver,
                PieceType.BackGold,
                PieceType.BackBishop,
                PieceType.BackRook,
                PieceType.BackKing,
            };
    }
}
