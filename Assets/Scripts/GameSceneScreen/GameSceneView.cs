using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameSceneView : MonoBehaviour
{
	[SerializeField] private Button openDebugMenuButton;
	[SerializeField] private Button closeDebugMenuButton;
	[SerializeField] private GameObject debugMenu;
    [SerializeField] private GameObject board;
    [SerializeField] private GameObject frontCapturePieceArea;
    [SerializeField] private GameObject backCapturePieceArea;
    [SerializeField] private PromotePopupView promotePopupView;

    public Button OpenDebugMenuButton => openDebugMenuButton;
	public Button CloseDebugMenuButton => closeDebugMenuButton;
	public GameObject DebugMenu => debugMenu;

	public GameObject Board => board;
	
	public GameObject FrontCapturePieceArea => frontCapturePieceArea;
	public GameObject BackCapturePieceArea => backCapturePieceArea;
	
	public PromotePopupView PromotePopupView => promotePopupView;

	// 新規対局時に駒台の表示を初期化
	public void ClearAllCapturePieceArea()
	{
		ClearCapturePieceArea(frontCapturePieceArea.transform);
        ClearCapturePieceArea(backCapturePieceArea.transform);
    }

	public void ClearCapturePieceArea(Transform capturePieceArea)
	{
        foreach (Transform child in capturePieceArea)
        {
            Destroy(child.gameObject);
        }
    }
}