using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriteEffect : MonoBehaviour
{
	public float delay = 0.1f;
	public string fullText;
	private string currentText = "";
	public TextMeshProUGUI textMeshProUGUI;

	// Use this for initialization
	void Start()
	{
		textMeshProUGUI = GetComponent<TextMeshProUGUI>();
		fullText = textMeshProUGUI.text;
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		for (int i = 0; i < fullText.Length; i++)
		{
			currentText = fullText.Substring(0, i+1);
			textMeshProUGUI.text = currentText;
			yield return new WaitForSecondsRealtime(delay);
		}
	}
}
