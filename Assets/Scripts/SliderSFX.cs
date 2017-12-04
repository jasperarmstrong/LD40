using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSFX : MonoBehaviour {
	void OnValueChanged(float value) {
		GameManager.volumeSFX = value;
		PlayerPrefs.SetFloat(GameManager.PP_VOLUME_SFX, value);
	}
	
	void Start () {
		Slider s = GetComponent<Slider>();
		s.value = GameManager.volumeSFX;
		s.onValueChanged.AddListener(OnValueChanged);
	}
}
