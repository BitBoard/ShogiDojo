using System;
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

    // ここでPieceTypeから文字列に変換する
    public static string PieceTypeToStr(PieceType pieceType, bool isAIFirst)
    {
        switch (pieceType)
        {
            case PieceType.BackPawn:
                return isAIFirst ? "BackPawn" : "FrontPawn";
            case PieceType.BackLance:
                return isAIFirst ? "BackLance" : "FrontLance";
            case PieceType.BackKnight:
                return isAIFirst ? "BackKnight" : "FrontKnight";
            case PieceType.BackSilver:
                return isAIFirst ? "BackSilver" : "FrontSilver";
            case PieceType.BackGold:
                return isAIFirst ? "BackGold" : "FrontGold";
            case PieceType.BackBishop:
                return isAIFirst ? "BackBishop" : "FrontBishop";
            case PieceType.BackRook:
                return isAIFirst ? "BackRook" : "FrontRook";
            case PieceType.BackKing:
                return isAIFirst ? "BackKing" : "FrontKing";
            case PieceType.FrontPawn:
                return isAIFirst ? "FrontPawn" : "BackPawn";
            case PieceType.FrontLance:
                return isAIFirst ? "FrontLance" : "BackLance";
            case PieceType.FrontKnight:
                return isAIFirst ? "FrontKnight" : "BackKnight";
            case PieceType.FrontSilver:
                return isAIFirst ? "FrontSilver" : "BackSilver";
            case PieceType.FrontGold:
                return isAIFirst ? "FrontGold" : "BackGold";
            case PieceType.FrontBishop:
                return isAIFirst ? "FrontBishop" : "BackBishop";
            case PieceType.FrontRook:
                return isAIFirst ? "FrontRook" : "BackRook";
            case PieceType.FrontKing:
                return isAIFirst ? "FrontKing" : "BackKing";
            default:
                return "None";
        }
    }

    // <summary>
    // 成駒の文字列を取得する
    // </summary>
    // <param name="pieceType"></param>
    // <returns></returns>
    public static string PieceTypeToPromoteStr(PieceType pieceType, bool isAIFirst)
    {
        switch (pieceType)
        {
            case PieceType.BackPawn:
                return isAIFirst ? "FrontPawnPromoted" : "BackPawnPromoted";
            case PieceType.BackLance:
                return isAIFirst ? "FrontLancePromoted" : "BackLancePromoted";
            case PieceType.BackKnight:
                return isAIFirst ? "FrontKnightPromoted" : "BackKnightPromoted";
            case PieceType.BackSilver:
                return isAIFirst ? "FrontSilverPromoted" : "BackSilverPromoted";
            case PieceType.BackBishop:
                return isAIFirst ? "FrontBishopPromoted" : "BackBishopPromoted";
            case PieceType.BackRook:
                return isAIFirst ? "FrontRookPromoted" : "BackRookPromoted";
            case PieceType.FrontPawn:
                return isAIFirst ? "BackPawnPromoted" : "FrontPawnPromoted";
            case PieceType.FrontLance:
                return isAIFirst ? "BackLancePromoted" : "FrontLancePromoted";
            case PieceType.FrontKnight:
                return isAIFirst ? "BackKnightPromoted" : "FrontKnightPromoted";
            case PieceType.FrontSilver:
                return isAIFirst ? "BackSilverPromoted" : "FrontSilverPromoted";
            case PieceType.FrontBishop:
                return isAIFirst ? "BackBishopPromoted" : "FrontBishopPromoted";
            case PieceType.FrontRook:
                return isAIFirst ? "BackRookPromoted" : "FrontRookPromoted";
            default:
                throw new Exception("成れない駒です。");
        }
    }

    public static PieceType[] getPieceTypeList(bool isBlack)
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
