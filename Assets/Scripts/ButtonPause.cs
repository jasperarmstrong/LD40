using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPause : MonoBehaviour {
	TextMeshProUGUI text;

	static string STR_RESUME = "RESUME (ESC)";
	static string STR_PAUSE  = "PAUSE (ESC)";

	void OnPause() {
		if (GameManager.IsPaused) {
			text.text = STR_RESUME;
		} else {
			text.text = STR_PAUSE;
		}
	}

	void OnClick() {
		GameManager.TogglePause();
	}
	
	void Start () {
		GameManager.OnPause += OnPause;
		text = GetComponentInChildren<TextMeshProUGUI>();
		GetComponent<Button>()?.onClick.AddListener(OnClick);
	}
}
