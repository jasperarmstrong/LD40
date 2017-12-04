using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMusic : MonoBehaviour {
	void OnValueChanged(float value) {
		GameManager.volumeMusic = value;
		PlayerPrefs.SetFloat(GameManager.PP_VOLUME_MUSIC, value);
	}
	
	void Start () {
		Slider s = GetComponent<Slider>();
		s.value = GameManager.volumeMusic;
		s.onValueChanged.AddListener(OnValueChanged);
	}
}
