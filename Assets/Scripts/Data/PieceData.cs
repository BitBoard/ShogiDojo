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
        // ここで文字列からswitch文でPieceTypeに変換する
        // 例: "white_pawn" -> PieceType.WhitePawn
        switch (pieceTypeStr)
        {
            case "white_pawn":
                return PieceType.WhitePawn;
            case "white_lance":
                return PieceType.WhiteLance;
            case "white_knight":
                return PieceType.WhiteKnight;
            case "white_silver":
                return PieceType.WhiteSilver;
            case "white_gold":
                return PieceType.WhiteGold;
            case "white_bishop":
                return PieceType.WhiteBishop;
            case "white_rook":
                return PieceType.WhiteRook;
            case "white_king":
                return PieceType.WhiteKing;
            case "black_pawn":
                return PieceType.BlackPawn;
            case "black_lance":
                return PieceType.BlackLance;
            case "black_knight":
                return PieceType.BlackKnight;
            case "black_silver":
                return PieceType.BlackSilver;
            case "black_gold":
                return PieceType.BlackGold;
            case "black_bishop":
                return PieceType.BlackBishop;
            case "black_rook":
                return PieceType.BlackRook;
            case "black_king":
                return PieceType.BlackKing;
            default:
                return PieceType.WhitePawn;
        }
    }
}
