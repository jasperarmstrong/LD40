using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	[SerializeField][Range(0f,1f)] float shakeAmount;
	[SerializeField][Range(0f,2f)] float shakeTime;

	IEnumerator ShakeCO() {
		float time = 0;
		Vector3 initPos = transform.localPosition;

		while (time < shakeTime) {
			transform.localPosition = initPos + (Vector3)Random.insideUnitCircle * shakeAmount;
			yield return null;
			time += Time.deltaTime;
		}

		transform.localPosition = initPos;
	}

	public void Shake() {
		StartCoroutine(ShakeCO());
	}
}
