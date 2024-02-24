using System;
using System.Collections;
using UnityEngine;

public partial class PieceData {
    public class PiecePosition
    {
        public int x;
        public int y;

        public PiecePosition(int x, int y) 
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
    
    public static string PieceTypeToStr(PieceType pieceType, bool isAIFirst)
    {
        // ここでPieceTypeから文字列に変換する
        // 例: PieceType.WhitePawn -> "white_pawn"
        switch (pieceType)
        {
            case PieceType.WhitePawn:
                return isAIFirst ? "white_pawn" : "black_pawn";
            case PieceType.WhiteLance:
                return isAIFirst ? "white_lance" : "black_lance";
            case PieceType.WhiteKnight:
                return isAIFirst ? "white_knight" : "black_knight";
            case PieceType.WhiteSilver:
                return isAIFirst ? "white_silver" : "black_silver";
            case PieceType.WhiteGold:
                return isAIFirst ? "white_gold" : "black_gold";
            case PieceType.WhiteBishop:
                return isAIFirst ? "white_bishop" : "black_bishop";
            case PieceType.WhiteRook:
                return isAIFirst ? "white_rook" : "black_rook";
            case PieceType.WhiteKing:
                return isAIFirst ? "white_king" : "black_king";
            case PieceType.BlackPawn:
                return isAIFirst ? "black_pawn" : "white_pawn";
            case PieceType.BlackLance:
                return isAIFirst ? "black_lance" : "white_lance";
            case PieceType.BlackKnight:
                return isAIFirst ? "black_knight" : "white_knight";
            case PieceType.BlackSilver:
                return isAIFirst ? "black_silver" : "white_silver";
            case PieceType.BlackGold:
                return isAIFirst ? "black_gold" : "white_gold";
            case PieceType.BlackBishop:
                return isAIFirst ? "black_bishop" : "white_bishop";
            case PieceType.BlackRook:
                return isAIFirst ? "black_rook" : "white_rook";
            case PieceType.BlackKing:
                return isAIFirst ? "black_king" : "white_king";
            default:
                return "none";
        }
    }

    /// <summary>
    /// 成駒の文字列を取得する
    /// </summary>
    /// <param name="pieceType"></param>
    /// <returns></returns>
    public static string PieceTypeToPromoteStr(PieceType pieceType, bool isAIFirst)
    {
        switch (pieceType)
        {
            case PieceType.WhitePawn:
                return isAIFirst? "black_prom_pawn" : "white_prom_pawn";
            case PieceType.WhiteLance:
                return isAIFirst ? "black_prom_lance" : "white_prom_lance";
            case PieceType.WhiteKnight:
                return isAIFirst ? "black_prom_knight" : "white_prom_knight";
            case PieceType.WhiteSilver:
                return isAIFirst ? "black_prom_silver" : "white_prom_silver";
            case PieceType.WhiteBishop:
                return isAIFirst ? "black_horse" : "white_horse";
            case PieceType.WhiteRook:
                return isAIFirst ? "black_dragon" : "white_dragon";
            case PieceType.BlackPawn:
                return isAIFirst ? "white_prom_pawn" : "black_prom_pawn";
            case PieceType.BlackLance:
                return isAIFirst ? "white_prom_lance" : "black_prom_lance";
            case PieceType.BlackKnight:
                return isAIFirst ? "white_prom_knight" : "black_prom_knight";
            case PieceType.BlackSilver:
                return isAIFirst ? "white_prom_silver" : "black_prom_silver";
            case PieceType.BlackBishop:
                return isAIFirst ? "white_horse" : "black_horse";
            case PieceType.BlackRook:
                return isAIFirst ? "white_dragon" : "black_dragon";
            default:
                throw new Exception("成れない駒です。");
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

    public static PieceType[] getPieceTypeList(bool isBlack)
    {
        return isBlack ?
            new PieceType[]
            {
                PieceType.BlackPawn,
                PieceType.BlackLance,
                PieceType.BlackKnight,
                PieceType.BlackSilver,
                PieceType.BlackGold,
                PieceType.BlackBishop,
                PieceType.BlackRook,
                PieceType.BlackKing
            } :
            new PieceType[]
            {
                PieceType.WhitePawn,
                PieceType.WhiteLance,
                PieceType.WhiteKnight,
                PieceType.WhiteSilver,
                PieceType.WhiteGold,
                PieceType.WhiteBishop,
                PieceType.WhiteRook,
                PieceType.WhiteKing
            };
    }
}
