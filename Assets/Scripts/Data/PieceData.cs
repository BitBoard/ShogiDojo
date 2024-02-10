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
    
    public static PieceType GetCapturePieceType(PieceType pieceType)
    {
        // ここで取った駒を取ったプレイヤーの駒に変換する
        // 例: PieceType.WhitePawn -> PieceType.BlackPawn
        switch (pieceType)
        {
            case PieceType.WhitePawn:
                return PieceType.BlackPawn;
            case PieceType.WhiteLance:
                return PieceType.BlackLance;
            case PieceType.WhiteKnight:
                return PieceType.BlackKnight;
            case PieceType.WhiteSilver:
                return PieceType.BlackSilver;
            case PieceType.WhiteGold:
                return PieceType.BlackGold;
            case PieceType.WhiteBishop:
                return PieceType.BlackBishop;
            case PieceType.WhiteRook:
                return PieceType.BlackRook;
            case PieceType.WhiteKing:
                return PieceType.BlackKing;
            case PieceType.BlackPawn:
                return PieceType.WhitePawn;
            case PieceType.BlackLance:
                return PieceType.WhiteLance;
            case PieceType.BlackKnight:
                return PieceType.WhiteKnight;
            case PieceType.BlackSilver:
                return PieceType.WhiteSilver;
            case PieceType.BlackGold:
                return PieceType.WhiteGold;
            case PieceType.BlackBishop:
                return PieceType.WhiteBishop;
            case PieceType.BlackRook:
                return PieceType.WhiteRook;
            case PieceType.BlackKing:
                return PieceType.WhiteKing;
            default:
                return PieceType.None;
        }
    }

    /// <summary>
    /// 取った駒を取ったプレイヤーの駒に変換する
    /// </summary>
    /// <param name="pieceType"></param>
    /// <returns></returns>
    public static string CapturePieceTypeToStr(PieceType pieceType)
    {
        return PieceTypeToStr(GetCapturePieceType(pieceType));
    }
}
