using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, IReceptacle {
	static float lerpTime = 0.13f;
	[SerializeField] Transform plateSpot;

	IEnumerator GoToTable(Transform obj) {
		obj.gameObject.layer = ConveyorVars.instance.LayerOnConveyor;

		float time = 0;
		Vector3 initPos = obj.position;
		Quaternion initRot = obj.rotation;

		while (time < lerpTime) {
			float progress = time / lerpTime;

			obj.position = Vector3.Lerp(initPos, plateSpot.position, progress);
			obj.rotation = Quaternion.Lerp(initRot, plateSpot.rotation, progress);

			yield return null;
			time += Time.deltaTime;
		}

		obj.position = plateSpot.position;
		obj.rotation = plateSpot.rotation;
	}

	public bool PlaceObject(Transform obj) {
		if (!obj.CompareTag(GameManager.TAG_PLATE)) return false;

		StartCoroutine(GoToTable(obj));

		return true;
	} 
}
