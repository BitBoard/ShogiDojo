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
    //public GameObject pieceNumText;
    public TextMeshProUGUI pieceNumText;
    //public TMP_Text pieceNumText;

    public Square SqPos => Converter.PosToSquare(piecePotition.x, piecePotition.y);
    public GameObject Outline => outline;
    
    void Start()
    {
        //pieceNumText = pieceNumText.GetComponent<TextMeshProUGUI>();
        //this.pieceNumText = GameObject.Find("PieceNumText");
        // 駒数の表示はデフォルトで非有効化する
        //pieceNumText.SetActive(false);
        pieceNumText.gameObject.SetActive(false);
    }

//   void Update()
//   {
//       if (IsCaptured())
//       {
//           pieceNumText.GetComponent<TextMeshProUGUI>().text = pieceNum;
//           Debug.Log(pieceNumText.GetComponent<TextMeshProUGUI>().text);
//       }
//   }

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
        if (IsCaptured() && pieceNum >= 2)
        {
            pieceNumText.text = pieceNum.ToString();
            //pieceNumText.SetActive(true);
            //pieceNumText.enabled = true;
            pieceNumText.gameObject.SetActive(true);
            Debug.Log(pieceNumText.text);
        }
        else
        {
            //pieceNumText.SetActive(false);
            //pieceNumText.enabled = false;
            pieceNumText.gameObject.SetActive(false);
        }
    }
}