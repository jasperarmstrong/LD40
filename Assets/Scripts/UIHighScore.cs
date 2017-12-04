using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHighScore : MonoBehaviour {
	void Start () {
		GetComponent<TextMeshProUGUI>().text = $"HIGH SCORE: {GameManager.HighScore}";
	}
}
