using MyShogi.Model.Shogi.Core;
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

    public UnityAction<string, BoardType> action;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(StartGame);
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void StartGame()
    {
        BoardType boardType;
        var dropPieceChoice = chooseDropPiece.value;
        string boardJsonPath;
        switch (dropPieceChoice)
        {
            case 0:
                boardJsonPath = GameConfig.initialBoardJsonPath;
                boardType = BoardType.NoHandicap;
                break;
            case 1:
                boardJsonPath = GameConfig.boardDropLeftLanceJsonPath;
                boardType = BoardType.HandicapKyo;
                break;
            case 2:
                boardJsonPath = GameConfig.boardDropRightLanceJsonPath;
                boardType = BoardType.HandicapRightKyo;
                break;
            case 3:
                boardJsonPath = GameConfig.boardDropBishopJsonPath;
                boardType = BoardType.HandicapKaku;
                break;
            case 4:
                boardJsonPath = GameConfig.boardDropRookJsonPath;
                boardType = BoardType.HandicapHisya;
                break;
            case 5:
                boardJsonPath = GameConfig.boardDropRookLanceJsonPath;
                boardType = BoardType.HandicapHisyaKyo;
                break;
            case 6:
                boardJsonPath = GameConfig.boardDropTwoJsonPath;
                boardType = BoardType.Handicap2;
                break;
            case 7:
                boardJsonPath = GameConfig.boardDropThreeJsonPath;
                boardType=BoardType.Handicap3;
                break;
            case 8:
                boardJsonPath = GameConfig.boardDropFourJsonPath;
                boardType = BoardType.Handicap4;
                break;
            case 9:
                boardJsonPath = GameConfig.boardDropFiveJsonPath;
                boardType=BoardType.Handicap5;
                break;
            case 10:
                boardJsonPath = GameConfig.boardDropLeftFiveJsonPath;
                boardType= BoardType.HandicapLeft5;
                break;
            case 11:
                boardJsonPath = GameConfig.boardDropSixJsonPath;
                boardType = BoardType.Handicap6;
                break;
            case 12:
                boardJsonPath = GameConfig.boardDropEightJsonPath;
                boardType = BoardType.Handicap8;
                break;
            case 13:
                boardJsonPath = GameConfig.boardDropTenJsonPath;
                boardType = BoardType.Handicap10;
                break;
            default:
                boardJsonPath = GameConfig.initialBoardJsonPath;
                boardType = BoardType.NoHandicap;
                break;
        }
        action.Invoke(boardJsonPath, boardType);
        ClosePanel();
    }

    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
}