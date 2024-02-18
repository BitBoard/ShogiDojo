using System.Collections;
using MyShogi.Model.Shogi.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject outline;
    public PieceType pieceType;
    public PieceData.PiecePotition piecePotition;
    public UnityAction OnClickAction;
    public bool isPromoted = false;
    public TextMeshProUGUI pieceNumText;

    public Square SqPos => Converter.PosToSquare(piecePotition.x, piecePotition.y);
    public GameObject Outline => outline;
    
   void Awake()
    {
        // 駒数の表示はデフォルトで非有効化する
        pieceNumText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(ToString());
        OnClickAction?.Invoke();
    }
    
    public bool IsTurnPlayerPiece(bool isBlackTurn)
    {
        var pieceTypeNum = (int) pieceType;
        if (isBlackTurn)
        {
            return pieceTypeNum >= 9;
        }
        
        return pieceTypeNum < 9;
    }

    public bool IsCaptured()
    {
        return piecePotition.x == -1 && piecePotition.y == -1;
    }

    public override string ToString()
    {
        return Converter.PosToSign(this.piecePotition.x, this.piecePotition.y) + " " + this.pieceType.ToString() +
               " x:" + this.piecePotition.x + " y:" + this.piecePotition.y;
    }

    public void IsActivePeiceNumText(int pieceNum)
    {
        // 駒の枚数表示テキストを更新
        pieceNumText.text = "×" + pieceNum.ToString();

        if (IsCaptured() && pieceNum >= 2)
        {
            pieceNumText.gameObject.SetActive(true);
            Debug.Log(pieceNumText.text);
        }
        else
        {
            pieceNumText.gameObject.SetActive(false);
        }
    }
}