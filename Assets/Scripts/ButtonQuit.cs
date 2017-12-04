using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonQuit : MonoBehaviour {
	void Start () {
		#if UNITY_WEBGL
		gameObject.SetActive(false);
		#else
		GetComponent<Button>().onClick.AddListener(Application.Quit);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
