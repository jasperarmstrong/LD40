using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorVars : MonoBehaviour {
	public static ConveyorVars instance;

	[SerializeField] Slider slider;

	public int LayerOnConveyor;
	public int LayerOffConveyor;

	public Sprite[] Sprites;
	[HideInInspector]
	public int SpriteCount;

	public static float minSpeed = 1;
	public static float maxSpeed = 3.5f;
	public static float speedInput = 0;
	public static float realSpeedInput = minSpeed;

	public static float secondsPerAnimFrameInput = 0.0315f;
	public static float realSecondsPerAnimFrameInput = secondsPerAnimFrameInput;

	void Awake() {
		instance = this;
		SpriteCount = Sprites.Length;
	}

	void Update() {
		realSpeedInput = Mathf.Lerp(minSpeed, maxSpeed, speedInput);
		speedInput = slider.value;

		#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.L)) realSpeedInput = 20;
		#endif
	}
}
