using UnityEngine;
using UnityEngine.UI;

public class PromotePopupView : MonoBehaviour
{
	[SerializeField] private Button promoteButton;
	[SerializeField] private Button cancelPromoteButton;
	
	public Button PromoteButton => promoteButton;
	public Button CancelPromoteButton => cancelPromoteButton;
}