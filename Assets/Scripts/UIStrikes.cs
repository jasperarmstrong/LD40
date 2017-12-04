using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class UIStrikes : MonoBehaviour {
	TextMeshProUGUI text;

	void Start () {
		GameManager.uiStrikes = this;
		text = GetComponent<TextMeshProUGUI>();
		UpdateStrikes();
	}

	public void UpdateStrikes() {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < GameManager.Strikes; i++) {
			if (i > GameManager.NumStrikesGameOver - 1) break;
			sb.Append('X');
		}
		for (int i = 0; i < GameManager.NumStrikesGameOver - GameManager.Strikes; i++) {
			sb.Append('_');
		}
		text.text = sb.ToString();
	}
}
