using MyShogi.Model.Shogi.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfigPopupController : MonoBehaviour
{
    [SerializeField] private TMP_Text firstPlayer;
    [SerializeField] private TMP_Text secondPlayer;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Dropdown chooseDropPiece;
    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button firstPlayerButton;
    [SerializeField] private Button secondPlayerButton;
    private bool isAIFirst = false;

    public UnityAction<string, BoardType, bool> action;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(StartGame);
        closeButton.onClick.AddListener(ClosePanel);
        firstPlayerButton.onClick.AddListener(ChooseFirstPlayer);
        secondPlayerButton.onClick.AddListener(ChooseSecondPlayer);
        chooseDropPiece.onValueChanged.AddListener(delegate { DropdownValueChanged(chooseDropPiece); });
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
        action.Invoke(boardJsonPath, boardType, isAIFirst);
        ClosePanel();
    }

    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }


    private void ChooseFirstPlayer()
    {
        isAIFirst = false;
        firstPlayerButton.image.color = new Color32(101, 173, 211, 255);
        secondPlayerButton.image.color = new Color32(255, 255, 255, 255);
        firstPlayer.text = "▲先手";
        secondPlayer.text = "△後手";
    }

    private void ChooseSecondPlayer()
    {
        isAIFirst = true;
        firstPlayerButton.image.color = new Color32(255, 255, 255, 255);
        secondPlayerButton.image.color = new Color32(101, 173, 211, 255);
        firstPlayer.text = "△後手";
        secondPlayer.text = "▲先手";
    }

    private void DropdownValueChanged(TMP_Dropdown change)
    {
        if (change.value == 0)
        {
            turnText.enabled = true;
            firstPlayerButton.gameObject.SetActive(true);
            secondPlayerButton.gameObject.SetActive(true);
            firstPlayer.text = "▲先手";
            secondPlayer.text = "△後手";
        }
        else
        {
            turnText.enabled = false;
            firstPlayerButton.gameObject.SetActive(false);
            secondPlayerButton.gameObject.SetActive(false);
            firstPlayer.text = "△後手";
            secondPlayer.text = "▲先手";
            isAIFirst = true;
        }
    }
}