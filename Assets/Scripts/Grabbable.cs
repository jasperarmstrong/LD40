using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IReceptacle {
	bool PlaceObject(Transform obj);
}

public class Grabbable : MonoBehaviour {
	static float moveTime = 0.15f;
	public bool isGrabbed = false;

	public Action OnGrab;

	Rigidbody2D rb;
	Collider2D col;
	SortingOrderManager som;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		som = GetComponent<SortingOrderManager>();
	}

	IEnumerator GoToParent() {
		float time = 0;
		Vector3 initPos = transform.localPosition;
		Quaternion initRot = transform.localRotation;
		Vector3 initScale = transform.localScale;
		
		while (time < moveTime) {
			float progress = time / moveTime;
			transform.localPosition = Vector3.Lerp(initPos, Vector3.zero, progress);
			transform.localRotation = Quaternion.Lerp(initRot, Quaternion.identity, progress);
			transform.localScale = Vector3.Lerp(initScale, Vector3.one, progress);
			yield return null;
			time += Time.deltaTime;
		}

		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	public void Grab(Transform newParent) {
		isGrabbed = true;
		rb.simulated = false;
		col.enabled = false;
		som.enabled = false;
		gameObject.layer = ConveyorVars.instance.LayerOffConveyor;
		transform.SetParent(newParent);
		OnGrab?.Invoke();
		StartCoroutine(GoToParent());
	}

	public void LetGo(IReceptacle rec = null) {
		isGrabbed = false;
		rb.simulated = true;
		col.enabled = true;
		som.enabled = true;
		transform.SetParent(null);
		rec?.PlaceObject(transform);
	}
}
