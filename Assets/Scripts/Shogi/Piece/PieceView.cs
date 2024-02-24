using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceView : MonoBehaviour
{
	[SerializeField] private GameObject outline;
	[SerializeField] private Image pieceImage;
	[SerializeField] private TextMeshProUGUI pieceNumText;

	public void SetActiveOutline(bool isShow)
	{
		outline.SetActive(isShow);
	}

	public void SetImage(Sprite image)
	{
		pieceImage.sprite = image;
	}

	public void SetActivePieceNumText(bool isActive)
	{
		pieceNumText.gameObject.SetActive(isActive);
	}
	
	public void SetPieceNumText(int pieceNum)
	{
		pieceNumText.text = pieceNum.ToString();
	}
		
}