using System.Collections;
using UnityEngine;

public partial class PieceData {
    public class PiecePotition
    {
        public int x;
        public int y;

        public PiecePotition(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }
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
                return PieceType.None;
        }
    }
    
    public static string PieceTypeToStr(PieceType pieceType)
    {
        // ここでPieceTypeから文字列に変換する
        // 例: PieceType.WhitePawn -> "white_pawn"
        switch (pieceType)
        {
            case PieceType.WhitePawn:
                return "white_pawn";
            case PieceType.WhiteLance:
                return "white_lance";
            case PieceType.WhiteKnight:
                return "white_knight";
            case PieceType.WhiteSilver:
                return "white_silver";
            case PieceType.WhiteGold:
                return "white_gold";
            case PieceType.WhiteBishop:
                return "white_bishop";
            case PieceType.WhiteRook:
                return "white_rook";
            case PieceType.WhiteKing:
                return "white_king";
            case PieceType.BlackPawn:
                return "black_pawn";
            case PieceType.BlackLance:
                return "black_lance";
            case PieceType.BlackKnight:
                return "black_knight";
            case PieceType.BlackSilver:
                return "black_silver";
            case PieceType.BlackGold:
                return "black_gold";
            case PieceType.BlackBishop:
                return "black_bishop";
            case PieceType.BlackRook:
                return "black_rook";
            case PieceType.BlackKing:
                return "black_king";
            default:
                return "none";
        }
    }

    /// <summary>
    /// 取った駒を取ったプレイヤーの駒に変換する
    /// </summary>
    /// <param name="pieceType"></param>
    /// <returns></returns>
    public static string CapturePieceTypeToStr(PieceType pieceType)
    {
        switch (pieceType)
        {
            case PieceType.WhitePawn:
                return "black_pawn";
            case PieceType.WhiteLance:
                return "black_lance";
            case PieceType.WhiteKnight:
                return "black_knight";
            case PieceType.WhiteSilver:
                return "black_silver";
            case PieceType.WhiteGold:
                return "black_gold";
            case PieceType.WhiteBishop:
                return "black_bishop";
            case PieceType.WhiteRook:
                return "black_rook";
            case PieceType.WhiteKing:
                return "black_king";
            case PieceType.BlackPawn:
                return "white_pawn";
            case PieceType.BlackLance:
                return "white_lance";
            case PieceType.BlackKnight:
                return "white_knight";
            case PieceType.BlackSilver:
                return "white_silver";
            case PieceType.BlackGold:
                return "white_gold";
            case PieceType.BlackBishop:
                return "white_bishop";
            case PieceType.BlackRook:
                return "white_rook";
            case PieceType.BlackKing:
                return "white_king";
            default:
                return "none";
        }
    }
}
