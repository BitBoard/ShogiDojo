using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfigPopupController : MonoBehaviour
{

    [SerializeField] private Dropdown chooseDropPiece;

    public UnityAction<string> action;

    public void StartGame()
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
    }
}