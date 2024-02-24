using System;
using MyShogi.Model.Shogi.Core;
using MSPiece = MyShogi.Model.Shogi.Core.Piece;

public class Converter
{
    /// <summary>
    /// ポジションデータを符号に変換する
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    /// <param name="isBlack"></param>
    /// <returns></returns>
    public static string PosToSign(int x, int y, bool isBlack = true)
    {
        string file = "";
        string rank = "";
        if (isBlack)
        {
            file = (9 - x).ToString();
            rank = NumToKanji(y + 1);
        }
        else
        {
            file = (x + 1).ToString();
            rank = NumToKanji(9 - y);
        }

        return file + rank;
    }

    /// <summary>
    /// 数字を漢数字に変換する
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string NumToKanji(int num)
    {
        switch (num)
        {
            case 1:
                return "一";
            case 2:
                return "二";
            case 3:
                return "三";
            case 4:
                return "四";
            case 5:
                return "五";
            case 6:
                return "六";
            case 7:
                return "七";
            case 8:
                return "八";
            case 9:
                return "九";
            default:
                return "";
        }
    }

    public static MSPiece PieceTypeToDropPiece(PieceType pieceType)
    {
        switch (pieceType)
        {
            case PieceType.FrontPawn:
            case PieceType.BackPawn:
                return MSPiece.PAWN;
            case PieceType.FrontLance:
            case PieceType.BackLance:
                return MSPiece.LANCE;
            case PieceType.FrontKnight:
            case PieceType.BackKnight:
                return MSPiece.KNIGHT;
            case PieceType.FrontSilver:
            case PieceType.BackSilver:
                return MSPiece.SILVER;
            case PieceType.FrontGold:
            case PieceType.BackGold:
                return MSPiece.GOLD;
            case PieceType.FrontBishop:
            case PieceType.BackBishop:
                return MSPiece.BISHOP;
            case PieceType.FrontRook:
            case PieceType.BackRook:
                return MSPiece.ROOK;
            default:
                throw new Exception("Invalid piece type");
        }
    }

    public static PieceType DropPieceToPieceType(MSPiece piece, bool isAIFirst)
    {
        switch (piece)
        {
            case MSPiece.PAWN:
                return isAIFirst ? PieceType.FrontPawn : PieceType.BackPawn;
            case MSPiece.LANCE:
                return isAIFirst ? PieceType.FrontLance : PieceType.BackLance;
            case MSPiece.KNIGHT:
                return isAIFirst ? PieceType.FrontKnight : PieceType.BackKnight;
            case MSPiece.SILVER:
                return isAIFirst ? PieceType.FrontSilver : PieceType.BackSilver;
            case MSPiece.GOLD:
                return isAIFirst ? PieceType.FrontGold : PieceType.BackGold;
            case MSPiece.BISHOP:
                return isAIFirst ? PieceType.FrontBishop : PieceType.BackBishop;
            case MSPiece.ROOK:
                return isAIFirst ? PieceType.FrontRook : PieceType.BackRook;
            default:
                throw new Exception("Invalid piece type");
        }
    }


    public static Square PosToSquare(int x, int y, bool isAIFirst)
    {
        // 筋はMyShogiが右上スタートで、本アプリでは左上スタート[0～8]で表現している。

        // fileはx座標で、何筋目かを表す。AIが後手番の場合、8-xで変換する必要がある。
        var file = isAIFirst ? (File)x : (File)8 - x;

        // rankはy座標で、何段目かを表す。AIが先手番の場合、8-yで変換する必要がある。
        var rank = isAIFirst ? (Rank)8 - y : (Rank)y;
        return Util.MakeSquare(file, rank);
    }

    public static int SquareToX(Square square)
    {
        var file = (int)square.ToFile();
        return 8 - file;
    }

    public static int SquareToY(Square square)
    {
        var rank = (int)square.ToRank();
        return rank;
    }

}