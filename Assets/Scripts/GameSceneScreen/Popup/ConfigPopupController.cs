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

    public UnityAction<string, BoardType> action;
    
    private bool isPlayerFirst = true;
    private bool isDropPiece = false;

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
        SetPlayerStatus();
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


    private void ChooseFirstPlayer()
    {
        isPlayerFirst = true;
        firstPlayerButton.image.color = new Color32(101, 173, 211, 255);
        secondPlayerButton.image.color = new Color32(255, 255, 255, 255);
    }

    private void ChooseSecondPlayer()
    {
        isPlayerFirst = false;
        firstPlayerButton.image.color = new Color32(255, 255, 255, 255);
        secondPlayerButton.image.color = new Color32(101, 173, 211, 255);
    }
    
    private void SetPlayerStatus()
    {
        if (isDropPiece)
        {
            // 現在は駒落ちなら必ずAIが先手にしている
            GameConfig.isAIFirst = true;
        }
        else
        {
            // 平手の場合、選択によって先手後手が変わる
            GameConfig.isAIFirst = !isPlayerFirst;
        }
        
        if (isPlayerFirst)
        {
            firstPlayer.text = isDropPiece ? "△上手" : "▲先手";
            secondPlayer.text = isDropPiece ? "▲下手" : "△後手";
        }
        else
        {
            firstPlayer.text = isDropPiece ? "▲下手" : "△後手";
            secondPlayer.text = isDropPiece ? "△上手" : "▲先手";
        }
    }

    private void DropdownValueChanged(TMP_Dropdown change)
    {
        if (change.value == 0)
        {
            isDropPiece = false;
            turnText.enabled = true;
            firstPlayerButton.gameObject.SetActive(true);
            secondPlayerButton.gameObject.SetActive(true);
        }
        else
        {
            isDropPiece = true;
            turnText.enabled = false;
            firstPlayerButton.gameObject.SetActive(false);
            secondPlayerButton.gameObject.SetActive(false);
        }
    }
}