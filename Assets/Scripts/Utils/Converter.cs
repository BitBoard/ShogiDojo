using System;
using MyShogi.Model.Shogi.Core;
using MSPiece = MyShogi.Model.Shogi.Core.Piece;

public class Converter
{
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

    public static PieceType DropPieceToPieceType(MSPiece piece)
    {
        switch (piece)
        {
            case MSPiece.PAWN:
                return GameConfig.isAIFirst ? PieceType.FrontPawn : PieceType.BackPawn;
            case MSPiece.LANCE:
                return GameConfig.isAIFirst ? PieceType.FrontLance : PieceType.BackLance;
            case MSPiece.KNIGHT:
                return GameConfig.isAIFirst ? PieceType.FrontKnight : PieceType.BackKnight;
            case MSPiece.SILVER:
                return GameConfig.isAIFirst ? PieceType.FrontSilver : PieceType.BackSilver;
            case MSPiece.GOLD:
                return GameConfig.isAIFirst ? PieceType.FrontGold : PieceType.BackGold;
            case MSPiece.BISHOP:
                return GameConfig.isAIFirst ? PieceType.FrontBishop : PieceType.BackBishop;
            case MSPiece.ROOK:
                return GameConfig.isAIFirst ? PieceType.FrontRook : PieceType.BackRook;
            default:
                throw new Exception("Invalid piece type");
        }
    }


    public static Square PosToSquare(int x, int y)
    {
        // 筋はMyShogiが右上スタートで、本アプリでは左上スタート[0～8]で表現している。

        // fileはx座標で、何筋目かを表す。AIが後手番の場合、8-xで変換する必要がある。
        var file = GameConfig.isAIFirst ? (File)x : (File)8 - x;

        // rankはy座標で、何段目かを表す。AIが先手番の場合、8-yで変換する必要がある。
        var rank = GameConfig.isAIFirst ? (Rank)8 - y : (Rank)y;
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