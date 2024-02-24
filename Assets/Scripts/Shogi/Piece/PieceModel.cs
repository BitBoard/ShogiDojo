using UniRx;

public class PieceModel
{
	private ReactiveProperty<PieceType> _pieceType;
	private ReactiveProperty<bool> _isFront;
	private ReactiveProperty<PieceData.PiecePosition> _piecePosition;
	private ReactiveProperty<bool> _isShowOutline;
	private ReactiveProperty<int> _pieceNum;

	public IReadOnlyReactiveProperty<PieceType> PieceType => _pieceType;
	public IReadOnlyReactiveProperty<PieceData.PiecePosition> PiecePosition => _piecePosition;
	public IReadOnlyReactiveProperty<bool> IsShowOutline => _isShowOutline;
	public IReadOnlyReactiveProperty<int> PieceNum => _pieceNum;

	public PieceModel()
	{
		_pieceType = new ReactiveProperty<PieceType>();
		_isFront = new ReactiveProperty<bool>();
		_piecePosition = new ReactiveProperty<PieceData.PiecePosition>();
		_isShowOutline = new ReactiveProperty<bool>();
		_pieceNum = new ReactiveProperty<int>();
	}

	public void SetPieceType(PieceType pieceType)
	{
		_pieceType.Value = pieceType;
	}
	
	public void SetPiecePosition(PieceData.PiecePosition piecePosition)
	{
		_piecePosition.Value = piecePosition;
	}
	
	public void SetIsShowOutline(bool isShow)
	{
		_isShowOutline.Value = isShow;
	}
	
	public void SetPieceNum(int pieceNum)
	{
		_pieceNum.Value = pieceNum;
	}
}