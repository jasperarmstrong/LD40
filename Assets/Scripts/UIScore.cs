using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour {
	TextMeshProUGUI text;
	static char zero = '0';

	void Start () {
		GameManager.uiScore = this;
		text = GetComponent<TextMeshProUGUI>();
		UpdateScore();
	}
	
	public void UpdateScore() {
		text.text = GameManager.Score.ToString().PadLeft(4, zero);
	}
}
