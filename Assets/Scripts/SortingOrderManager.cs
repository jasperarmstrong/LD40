using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class SortingOrderManager : MonoBehaviour {
	[SerializeField] SpriteRenderer spriteRenderer;
	public Action<int> OnFinalizeSortingOrder;
	public int offset = 0;
	void LateUpdate () {
		if (spriteRenderer != null) spriteRenderer.sortingOrder = (int)(-transform.position.y * 10) + offset;
		OnFinalizeSortingOrder?.Invoke(spriteRenderer.sortingOrder);
	}
}
