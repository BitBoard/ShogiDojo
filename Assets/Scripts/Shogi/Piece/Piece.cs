using MyShogi.Model.Shogi.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 駒の振る舞いを定義するクラス
/// </summary>
public class Piece : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private PieceView view;
	private PieceModel model;
	public UnityAction OnClickAction;
	
	private void Awake()
	{
		Init();
	}

	/// <summary>
	/// 初期化を行う
	/// </summary>
	private void Init()
	{
		model = new PieceModel();
		Bind();
	}

	private void Bind()
	{
		model.IsShowOutline.Subscribe(isShow => view.SetActiveOutline(isShow));
		model.PieceType.Subscribe(UpdatePieceImage);
		model.PieceNum.Subscribe(UpdatePieceNumText);
	}
	

	public Square SqPos(bool isAIFirst)
	{
		return Converter.PosToSquare(model.PiecePosition.Value.x, model.PiecePosition.Value.y, isAIFirst);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log(ToString());
		OnClickAction?.Invoke();
	}

	/// <summary>
	/// 駒を成る処理
	/// </summary>
	public void Promotion()
	{
		switch (model.PieceType)
		{
			// 駒を成る処理を実装する
		}
	}
	
	public PieceType GetPieceType()
	{
		return model.PieceType.Value;
	}
	
	public void SetPieceType(PieceType pieceType)
	{
		model.SetPieceType(pieceType);
	}
	
	public PieceData.PiecePosition GetPiecePosition()
	{
		return model.PiecePosition.Value;
	}
	
	/// <summary>
	/// 駒の位置を指定する
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void SetPiecePosition(int x, int y)
	{
		model.SetPiecePosition(new PieceData.PiecePosition(x, y));
	}
	
	/// <summary>
	/// 駒の枚数を取得する
	/// </summary>
	/// <returns></returns>
	public int GetPieceNum()
	{
		return model.PieceNum.Value;
	}
	
	/// <summary>
	/// 駒の枚数を設定する
	/// </summary>
	/// <param name="pieceNum"></param>
	public void SetPieceNum(int pieceNum)
	{
		model.SetPieceNum(pieceNum);
	}
	
	/// <summary>
	/// アウトラインの表示を設定する
	/// </summary>
	/// <param name="isShow"></param>
	public void SetIsShowOutline(bool isShow)
	{
		model.SetIsShowOutline(isShow);
	}

	
	public bool IsTurnPlayerPiece(bool isBlackTurn)
	{
		var pieceTypeNum = (int) model.PieceType.Value;
		if (isBlackTurn)
		{
			return pieceTypeNum <= 15;
		}
		
		return pieceTypeNum > 15;
	}

	public bool IsCaptured()
	{
		return model.PiecePosition.Value.x == -1 && model.PiecePosition.Value.y == -1;
	}

	public override string ToString()
	{
		return Converter.PosToSign(model.PiecePosition.Value.x, model.PiecePosition.Value.y) + " " + model.PieceType.Value +
			   " x:" + model.PiecePosition.Value.x + " y:" + model.PiecePosition.Value.y;
	}

	private void UpdatePieceImage(PieceType pieceType)
	{
		// Managementクラスから駒の画像を取得して設定する
		// var sprite = Management.Instance.GetPieceSprite(pieceType);
		// view.SetImage(sprite);
	}

	private void UpdatePieceNumText(int pieceNum)
	{
		Debug.Log("駒の枚数:" + pieceNum + "枚");
		if (pieceNum >= 2)
		{
			view.SetActivePieceNumText(true);
			view.SetPieceNumText(pieceNum);
		}
		else
		{
			view.SetActivePieceNumText(false);
			view.SetPieceNumText(pieceNum);
		}
	}
}