using System;
using TMPro;
using UnityEngine;

public class Blinker : MonoBehaviour
{
	private TextMeshProUGUI text;
	
	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}
	
	private void Update()
	{
		// 文字のアルファ値を正弦波で変化させる
		var alpha = Mathf.Sin(Time.time * 2 * Mathf.PI) * 0.5f + 0.5f;
		text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
	}
}