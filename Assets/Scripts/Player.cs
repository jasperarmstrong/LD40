using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {
	float moveSpeed = 8;
	float moveLerpFactor = 0.25f;

	float h, v;

	[SerializeField] LayerMask grabbableLayers;
	[SerializeField] LayerMask receptacleLayers;

	bool facingForward = true;
	[SerializeField] Sprite[] spritesFront;
	[SerializeField] Sprite[] spritesBack;
	Sprite[] spritesCurrent;
	int spriteCount = 0;
	int spriteIndex = 0;
	float spriteTimePerFrame = 0.04f;
	float spriteTime = 0;

	[SerializeField] Transform handRFront;
	[SerializeField] Transform handRBack;

	[SerializeField] Transform spriteTransform;
	SpriteRenderer sr; 
	Rigidbody2D rb;
	SortingOrderManager som;

	Collider2D[] hits;
	Grabbable heldItem = null;

	void Start () {
		sr = spriteTransform.GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		som = GetComponent<SortingOrderManager>();

		spriteCount = spritesFront.Length;
		spritesCurrent = spritesFront;
	}

	float GrabOrderFunc(Collider2D col) {
		return (col.transform.position - transform.position).magnitude;
	}

	IReceptacle GetNearestReceptacle() {
		IReceptacle result = null;
		if (heldItem == null) return result;

		hits = Physics2D.OverlapCircleAll(transform.position, 0.6f, receptacleLayers).OrderBy(GrabOrderFunc).ToArray();
		foreach (Collider2D col in hits) {
			result = col.GetComponentInParent<IReceptacle>();
			if (result != null) break;
		}

		return result;
	}

	void GrabNearestItem() {
		if (heldItem != null) return;

		hits = Physics2D.OverlapCircleAll(transform.position, 1, grabbableLayers).OrderBy(GrabOrderFunc).ToArray();
		if (hits.Length > 0) {
			heldItem = hits[0].GetComponent<Grabbable>();
			if (heldItem != null) heldItem.Grab(facingForward ? handRFront : handRBack);
			som.OnFinalizeSortingOrder += UpdateHeldItemLayer;
		}
	}

	void UpdateHeldItemLayer(int sortingOrder) {
		if (heldItem == null) {
			som.OnFinalizeSortingOrder -= UpdateHeldItemLayer;
			return;
		}
		SpriteRenderer rend = heldItem.GetComponent<SpriteRenderer>();
		if (rend == null) {
			som.OnFinalizeSortingOrder -= UpdateHeldItemLayer;
			return;
		}
		rend.sortingOrder = sortingOrder + (facingForward ? 1 : -1);

		SortingOrderManager heldSOM = heldItem.GetComponent<SortingOrderManager>();
		heldSOM?.OnFinalizeSortingOrder?.Invoke(facingForward ? rend.sortingOrder : rend.sortingOrder - 4);
	}

	void UpdateHeldItemPos() {
		if (heldItem == null) return;
		heldItem.transform.SetParent(facingForward ? handRFront : handRBack);
		heldItem.transform.localPosition = Vector3.zero;
		heldItem.transform.localRotation = Quaternion.identity;
	}

	void UpdateSpriteIndex() {
		spriteTime += Time.deltaTime;
		if (spriteTime >= spriteTimePerFrame * (moveSpeed / rb.velocity.magnitude)) {
			spriteTime = 0;
			spriteIndex++;
			if (spriteIndex > spriteCount - 1) spriteIndex = 0;
		}
	}
	
	void Update () {
		if (GameManager.IsPaused || GameManager.IsGameOver) {
			return;
		}

		h = Input.GetAxisRaw(GameManager.AXIS_HOR);
		v = Input.GetAxisRaw(GameManager.AXIS_VER);

		Vector3 scale = spriteTransform.localScale;
		if (h < 0) scale.x = -1;
		else if (h > 0) scale.x = 1;
		spriteTransform.localScale = scale;

		UpdateSpriteIndex();

		if (v < 0) {
			spritesCurrent = spritesFront;
			facingForward = true;
		} else if (v > 0) {
			spritesCurrent = spritesBack;
			facingForward = false;
		}
		sr.sprite = spritesCurrent[spriteIndex];

		UpdateHeldItemPos();

		if (Input.GetKeyDown(GameManager.KC_GRAB) || Input.GetMouseButtonDown(GameManager.INT_GRAB_ALT)) {
			if (heldItem == null) GrabNearestItem();
			else {
				heldItem.LetGo(GetNearestReceptacle());
				heldItem = null;
				som.OnFinalizeSortingOrder -= UpdateHeldItemLayer;
			}
		}
	}

	void FixedUpdate() {
		if (GameManager.IsGameOver) {
			rb.velocity = Vector2.zero;
			return;
		}
		rb.velocity = Vector3.ClampMagnitude(Vector3.Lerp(rb.velocity, new Vector2(h, v) * moveSpeed, moveLerpFactor), moveSpeed);
	}
}
