using TMPro;
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
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button resignButton;
    [SerializeField] private GameObject thinkingObject;

    public Button OpenDebugMenuButton => openDebugMenuButton;
	public Button CloseDebugMenuButton => closeDebugMenuButton;
	public GameObject DebugMenu => debugMenu;

	public GameObject Board => board;
	
	public GameObject FrontCapturePieceArea => frontCapturePieceArea;
	public GameObject BackCapturePieceArea => backCapturePieceArea;
	
	public PromotePopupView PromotePopupView => promotePopupView;
	
	public GameObject ResultPanel => resultPanel;
	
	public TextMeshProUGUI ResultText => resultText;
	
	public Button RetryButton => retryButton;
	
	public Button ResetButton => resetButton;
	
	public Button ResignButton => resignButton;
	
	public GameObject ThinkingObject => thinkingObject;

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
	
	// 結果を設定
	public void SetResult(bool isPlayerWin)
	{
		string result = isPlayerWin ? "あなたの勝ち！！" : "あなたの負け...";
		resultText.text = result;
	}
	
}