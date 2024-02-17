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
	
	/// <summary>
	/// PieceTypeをMyShogiのPieceに変換する
	/// </summary>
	/// <param name="pieceType"></param>
	/// <param name="isPromoted"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static MSPiece PieceTypeToPiece(PieceType pieceType, bool isPromoted)
	{
		if (isPromoted)
		{
			switch (pieceType)
			{
				case PieceType.BlackPawn:
					return MSPiece.B_PRO_PAWN;
				case PieceType.BlackLance:
					return MSPiece.B_PRO_LANCE;
				case PieceType.BlackKnight:
					return MSPiece.B_PRO_KNIGHT;
				case PieceType.BlackSilver:
					return MSPiece.B_PRO_SILVER;
				case PieceType.BlackRook:
					return MSPiece.B_DRAGON;
				case PieceType.BlackBishop:
					return MSPiece.B_HORSE;
				case PieceType.WhitePawn:
					return MSPiece.W_PRO_PAWN;
				case PieceType.WhiteLance:
					return MSPiece.W_PRO_LANCE;
				case PieceType.WhiteKnight:
					return MSPiece.W_PRO_KNIGHT;
				case PieceType.WhiteSilver:
					return MSPiece.W_PRO_SILVER;
				case PieceType.WhiteRook:
					return MSPiece.W_DRAGON;
				case PieceType.WhiteBishop:
					return MSPiece.W_HORSE;
				default:
					throw new Exception("Invalid piece type");
			}
		}
		
		switch (pieceType)
		{
			case PieceType.BlackPawn:
				return MSPiece.B_PAWN;
			case PieceType.BlackLance:
				return MSPiece.B_LANCE;
			case PieceType.BlackKnight:
				return MSPiece.B_KNIGHT;
			case PieceType.BlackSilver:
				return MSPiece.B_SILVER;
			case PieceType.BlackGold:
				return MSPiece.B_GOLD;
			case PieceType.BlackRook:
				return MSPiece.B_ROOK;
			case PieceType.BlackBishop:
				return MSPiece.B_BISHOP;
			case PieceType.BlackKing:
				return MSPiece.B_KING;
			case PieceType.WhitePawn:
				return MSPiece.W_PAWN;
			case PieceType.WhiteLance:
				return MSPiece.W_LANCE;
			case PieceType.WhiteKnight:
				return MSPiece.W_KNIGHT;
			case PieceType.WhiteSilver:
				return MSPiece.W_SILVER;
			case PieceType.WhiteGold:
				return MSPiece.W_GOLD;
			case PieceType.WhiteRook:
				return MSPiece.W_ROOK;
			case PieceType.WhiteBishop:
				return MSPiece.W_BISHOP;
			case PieceType.WhiteKing:
				return MSPiece.W_KING;
			default:
				throw new Exception("Invalid piece type");
		}
	}

	public static MSPiece PieceTypeToDropPiece(PieceType pieceType)
	{
		switch (pieceType)
		{
			case PieceType.BlackPawn:
			case PieceType.WhitePawn:
				return MSPiece.PAWN;
			case PieceType.BlackLance:
			case PieceType.WhiteLance:
				return MSPiece.LANCE;
			case PieceType.BlackKnight:
			case PieceType.WhiteKnight:
				return MSPiece.KNIGHT;
			case PieceType.BlackSilver:
			case PieceType.WhiteSilver:
				return MSPiece.SILVER;
			case PieceType.BlackGold:
			case PieceType.WhiteGold:
				return MSPiece.GOLD;
			case PieceType.BlackBishop:
			case PieceType.WhiteBishop:
				return MSPiece.BISHOP;
			case PieceType.BlackRook:
			case PieceType.WhiteRook:
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
				return isAIFirst ? PieceType.BlackPawn : PieceType.WhitePawn;
			case MSPiece.LANCE:
				return isAIFirst ? PieceType.BlackLance : PieceType.WhiteLance;
			case MSPiece.KNIGHT:
				return isAIFirst ? PieceType.BlackKnight : PieceType.WhiteKnight;
			case MSPiece.SILVER:
				return isAIFirst ? PieceType.BlackSilver : PieceType.WhiteSilver;
			case MSPiece.GOLD:
				return isAIFirst ? PieceType.BlackGold : PieceType.WhiteGold;
			case MSPiece.BISHOP:
				return isAIFirst ? PieceType.BlackBishop : PieceType.WhiteBishop;
			case MSPiece.ROOK:
				return isAIFirst ? PieceType.BlackRook : PieceType.WhiteRook;
			default:
				throw new Exception("Invalid piece type");
		}
	}


	public static Square PosToSquare(int x, int y)
	{
		var file = (File)(8 - x);
		var rank = (Rank)(y);
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