using UnityEngine;
using UnityEngine.UI;

public class GameSceneView : MonoBehaviour
{
	[SerializeField] private Button openDebugMenuButton;
	[SerializeField] private Button closeDebugMenuButton;
	[SerializeField] private GameObject debugMenu;
    [SerializeField] private GameObject board;

    public Button OpenDebugMenuButton => openDebugMenuButton;
	public Button CloseDebugMenuButton => closeDebugMenuButton;
	public GameObject DebugMenu => debugMenu;

	public GameObject Board => board;
}