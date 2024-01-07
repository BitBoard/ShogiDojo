using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerClickHandler
{
    public PieceData.PieceType pieceType;
    public PieceData.PiecePotition piecePotition;
    public UnityAction OnClickAction;
    

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(ToString());
        OnClickAction.Invoke();
    }

    public override string ToString()
    {
        return Converter.PosToSign(this.piecePotition.x, this.piecePotition.y) + " " + this.pieceType.ToString() +
               " x:" + this.piecePotition.x + " y:" + this.piecePotition.y;
    }
}