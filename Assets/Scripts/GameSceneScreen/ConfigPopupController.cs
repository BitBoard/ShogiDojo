using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfigPopupController : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown chooseDropPiece;
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button closeButton;

    public UnityAction<string> action;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(StartGame);
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void StartGame()
    {
        var dropPieceChoice = chooseDropPiece.value;
        string boardJsonPath;
        switch (dropPieceChoice)
        {
            case 0:
                boardJsonPath = GameConfig.initialBoardJsonPath; break;
            case 1:
                boardJsonPath = GameConfig.boardDropRookJsonPath; break;
            default:
                boardJsonPath = GameConfig.initialBoardJsonPath; break;
        }
        action.Invoke(boardJsonPath);
        ClosePanel();
    }

    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
}