using System.Collections;
using MyShogi.Model.Shogi.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject outline;
    public PieceType pieceType;
    public PieceData.PiecePotition piecePotition;
    public UnityAction OnClickAction;
    
    public Square SqPos => Converter.PosToSquare(piecePotition.x, piecePotition.y);
    public GameObject Outline => outline;
    

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(ToString());
        OnClickAction?.Invoke();
    }

    public override string ToString()
    {
        return Converter.PosToSign(this.piecePotition.x, this.piecePotition.y) + " " + this.pieceType.ToString() +
               " x:" + this.piecePotition.x + " y:" + this.piecePotition.y;
    }
}