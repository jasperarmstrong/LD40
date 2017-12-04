using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour, IReceptacle {
	[SerializeField] Transform[] spots;
	List<Transform> sushis;
	int numSushis = 0;

	SortingOrderManager som;
	Grabbable g;

	void FixSortingOrders(int sortingOrder) {
		foreach(Transform t in sushis) {
			t.GetComponent<SpriteRenderer>().sortingOrder = ++sortingOrder;
		}
	}

	void Start () {
		sushis = new List<Transform>();
		som = GetComponent<SortingOrderManager>();
		som.OnFinalizeSortingOrder += FixSortingOrders;
		g = GetComponent<Grabbable>();
		g.OnGrab += SpawnNewPlate;
	}

	void SpawnNewPlate() {
		g.OnGrab -= SpawnNewPlate;
		PlateSpawner.instance.SpawnPlate();
	}

	public bool PlaceObject(Transform obj) {
		if (numSushis > spots.Length - 1 || !obj.CompareTag(GameManager.TAG_SUSHI)) return false;

		obj.GetComponent<Grabbable>()?.Grab(spots[numSushis]);
		sushis.Add(obj);
		numSushis++;

		return true;
	}
}
