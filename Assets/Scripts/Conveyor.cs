using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour, IReceptacle {
	float time = 0;

	int spriteIndex = 0;

	[SerializeField] SpriteRenderer conveyorSpriteRenderer;

	List<Grabbable> sushis;
	List<Grabbable> toBeRemoved;

	void Start() {
		sushis = new List<Grabbable>();
		toBeRemoved = new List<Grabbable>();
	}

	void UpdateSprite() {
		ConveyorVars.realSecondsPerAnimFrameInput = ConveyorVars.secondsPerAnimFrameInput * (1 / ConveyorVars.realSpeedInput);
		
		if (time >= ConveyorVars.realSecondsPerAnimFrameInput) {
			time = 0;
			if (++spriteIndex > ConveyorVars.instance.SpriteCount - 1) spriteIndex = 0;
			conveyorSpriteRenderer.sprite = ConveyorVars.instance.Sprites[spriteIndex];
		}
	}
	
	void LateUpdate () {
		UpdateSprite();
		time += Time.deltaTime;
	}

	void FixedUpdate() {
		foreach (Grabbable g in sushis) {
			if (g.isGrabbed && !toBeRemoved.Contains(g)) toBeRemoved.Add(g);
			g.GetComponent<Rigidbody2D>().velocity = -transform.right * ConveyorVars.realSpeedInput;
		}
		foreach (Grabbable g in toBeRemoved) {
			if (sushis.Contains(g)) sushis.Remove(g);
		}
	}

	public bool PlaceObject(Transform obj, bool force = false) {
		if (force || true) {
			Grabbable g = obj.GetComponent<Grabbable>();
			if (g != null && !sushis.Contains(g)) sushis.Add(g);
			obj.gameObject.layer = ConveyorVars.instance.LayerOnConveyor;
			obj.position = transform.position + Random.Range(0.12f, 0.16f) * Vector3.up;
			return true;
		}
		return false;
	}

	public bool PlaceObject(Transform obj) {
		return PlaceObject(obj, false);
	}

	void OnTriggerEnter2D(Collider2D col) {
		Grabbable g = col.GetComponent<Grabbable>();
		if (g != null && !sushis.Contains(g)) sushis.Add(g);
	}

	void OnTriggerExit2D(Collider2D col) {
		Grabbable g = col.GetComponent<Grabbable>();
		if (g != null && sushis.Contains(g)) sushis.Remove(g);
	}
}
