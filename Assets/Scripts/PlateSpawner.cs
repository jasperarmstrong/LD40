using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSpawner : MonoBehaviour {
	public static PlateSpawner instance;
	[SerializeField] GameObject prefab;
	[SerializeField] Transform spawnSpot;

	static float lerpTime = 0.5f;

	void Awake() {
		instance = this;
	}

	IEnumerator SpawnPlateCO(Transform t) {
		t.localScale = Vector3.zero;
		float time = 0;

		while (time < lerpTime) {
			t.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / lerpTime);
			yield return null;
			time += Time.deltaTime;
		}

		t.localScale = Vector3.one;
	}

	public void SpawnPlate() {
		StartCoroutine(SpawnPlateCO(((GameObject)Instantiate(prefab, spawnSpot.position, Quaternion.identity)).transform));
	}
}
