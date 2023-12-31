using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private GameObject highlight;
	public int x;
	public int y;
	public UnityAction OnClickAction;
	
	
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Cell clicked");
		OnClickAction.Invoke();
	}
}