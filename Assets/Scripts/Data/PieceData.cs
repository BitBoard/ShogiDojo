using System.Collections;
using UnityEngine;

public class PieceData {
    public enum PieceType
    {
        WhitePawn,
        WhiteLance,
        WhiteKnight,
        WhiteSilver,
        WhiteGold,
        WhiteBishop,
        WhiteRook,
        WhiteKing,
        BlackPawn,
        BlackLance,
        BlackKnight,
        BlackSilver,
        BlackGold,
        BlackBishop,
        BlackRook,
        BlackKing
    }

    public static PieceType StrToPieceType(string pieceTypeStr)
    {
        return PieceType.WhitePawn;
    }
}
