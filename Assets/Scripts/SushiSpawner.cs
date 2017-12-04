using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiSpawner : MonoBehaviour {
	[SerializeField] Conveyor conveyor;
	[SerializeField] GameObject[] prefabs;
	float time = 0;

	static float minInterval = 6;
	static float maxInterval = 0.9f;
	float currentInterval = minInterval;

	void SpawnSushi() {
		conveyor.PlaceObject(((GameObject)Instantiate(prefabs[Random.Range(0, prefabs.Length)], conveyor.transform.position, Quaternion.identity)).transform, true);
	}

	void Start() {
		SpawnSushi();
	}
	
	void LateUpdate () {
		currentInterval = Mathf.Lerp(minInterval, maxInterval, ConveyorVars.speedInput);
		#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.L)) currentInterval = 0.1f;
		#endif
		
		if (time >= currentInterval) {
			time = 0;
			SpawnSushi();
		}

		time += Time.deltaTime;
	}
}
