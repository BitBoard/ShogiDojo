using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerClickHandler
{
    public PieceData.PieceType pieceType;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.pieceType.ToString());
    }
}