using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertLayerOnTrigger : MonoBehaviour {
	[SerializeField] int fromLayer;
	[SerializeField] int toLayer;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == fromLayer) col.gameObject.layer = toLayer;
	}
}
