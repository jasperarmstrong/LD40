using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IReceptacle {
	static float destroyTime = 0.2f;
	[SerializeField] Transform spot;

	SpriteRenderer sr;

	void Start() {
		sr = GetComponent<SpriteRenderer>();
	}

	IEnumerator FadeToOblivion(Transform obj) {
		float time = 0;
		Vector3 initPos = obj.position;
		Quaternion initRot = obj.rotation;
		Vector3 initScale = obj.localScale;

		SortingOrderManager som = obj.GetComponent<SortingOrderManager>();
		if (som != null) {
			som.enabled = false;
			SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
			if (rend != null) {
				rend.sortingOrder = sr.sortingOrder + 1;
			}
		}

		while (time < destroyTime) {
			float progress = time / destroyTime;
			obj.position = Vector3.Lerp(initPos, spot.position, progress);
			obj.rotation = Quaternion.Lerp(initRot, spot.rotation, progress);
			obj.localScale = Vector3.Lerp(initScale, Vector3.zero, progress);
			yield return null;
			time += Time.deltaTime;
		}

		Destroy(obj.gameObject);
	}

	public bool PlaceObject(Transform obj) {
		StartCoroutine(FadeToOblivion(obj));
		return true;
	}
}
