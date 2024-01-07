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
}