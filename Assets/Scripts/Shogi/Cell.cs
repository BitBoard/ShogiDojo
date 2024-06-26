using System;
using System.Collections;
using System.Collections.Generic;
using MyShogi.Model.Shogi.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler, ISelectableTarget
{
	[SerializeField] private GameObject highlight;
	public int x;
	public int y;
	public UnityAction OnClickAction;
	
	public Square SqPos()
	{
        return Converter.PosToSquare(x, y);
    }
	
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Cell clicked");
		OnClickAction.Invoke();
	}
	
	public void SetHighlight(bool isHighlighted)
	{
		highlight.SetActive(isHighlighted);
	}
}