using UnityEngine;
using UnityEngine.UI;

public class GameSceneView : MonoBehaviour
{
	[SerializeField] private Button openDebugMenuButton;
	[SerializeField] private Button closeDebugMenuButton;
	[SerializeField] private GameObject debugMenu;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject blackCapturePieceArea;
    [SerializeField] private GameObject whiteCapturePieceArea;
    [SerializeField] private PromotePopupView promotePopupView;

    public Button OpenDebugMenuButton => openDebugMenuButton;
	public Button CloseDebugMenuButton => closeDebugMenuButton;
	public GameObject DebugMenu => debugMenu;

	public GameObject Board => board;
	
	public GameObject BlackCapturePieceArea => blackCapturePieceArea;
	public GameObject WhiteCapturePieceArea => whiteCapturePieceArea;
	
	public PromotePopupView PromotePopupView => promotePopupView;

	// V‹K‘Î‹Ç‚É‹î‘ä‚Ì•\¦‚ğ‰Šú‰»
	public void ClearAllCapturePieceArea()
	{
		ClearCapturePieceArea(blackCapturePieceArea.transform);
        ClearCapturePieceArea(whiteCapturePieceArea.transform);
    }

	public void ClearCapturePieceArea(Transform capturePieceArea)
	{
        foreach (Transform child in capturePieceArea)
        {
            Destroy(child.gameObject);
        }
    }
}