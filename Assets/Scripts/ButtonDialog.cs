using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDialog : MonoBehaviour {
	[SerializeField][TextArea(15,20)] string text;

	void ShowDialog() {
		GameManager.Dialog(text);
	}

	void Start () {
		GetComponent<Button>()?.onClick.AddListener(ShowDialog);
	}
}
