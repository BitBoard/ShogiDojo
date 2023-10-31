using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private GameObject highlight;
	
	
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Cell clicked");
		highlight.SetActive(!highlight.activeSelf);
	}
}