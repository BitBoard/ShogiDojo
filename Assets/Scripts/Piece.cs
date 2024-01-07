using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerClickHandler
{
    public PieceData.PieceType pieceType;
    public PieceData.PicecePotition picecePotition;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(Converter.PosToSign(this.picecePotition.x, this.picecePotition.y) + " " + this.pieceType.ToString() + " x:" +this.picecePotition.x + " y:" + this.picecePotition.y);
    }
}