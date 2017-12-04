using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderUI : MonoBehaviour {
	[SerializeField] Image[] images;
	public Image ProgressBar;
	[SerializeField] Image Background;
	
	public void SupplyValues(Dictionary<SushiType, int> sushis) {
		int num = 0;
		foreach (SushiType st in System.Enum.GetValues(typeof(SushiType))) {
			for (int i = 0; i < sushis[st]; i++) {
				images[num].sprite = GameManager.SushiSprites[st];
				images[num].gameObject.SetActive(true);
				num++;
			}
		}
	}

	IEnumerator FailCO() {
		Background.color = Color.red;
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

	public void Fail() {
		StartCoroutine(FailCO());
	}

	IEnumerator SucceedCO() {
		Background.color = Color.green;
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}

	public void Succeed() {
		StartCoroutine(SucceedCO());
	}
}
