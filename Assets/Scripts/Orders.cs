using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Order {
	public Dictionary<SushiType, int> Items;
	public float initTime;
	public float timeLeft;
	public OrderUI orderUI;
	public bool dead = false;

	public Order() {
		Items = new Dictionary<SushiType, int>();
		Reset();
	}

	public void Reset() {
		Zero();
		orderUI = null;
		dead = false;

		int numLeft = 3;
		int initNumLeft = numLeft;

		while (numLeft == initNumLeft) {
			foreach (SushiType st in System.Enum.GetValues(typeof(SushiType))) {
				Items[st] = Random.Range(0, numLeft + 1);
				numLeft -= Items[st];
			}
		}

		initTime = timeLeft = Mathf.Lerp(Orders.maxAmtTime, Orders.minAmtTime, Random.Range(0f, 1f) * Mathf.Clamp(GameManager.Score / GameManager.ScoreMaxDifficulty, 0.4f, 1f));
		orderUI = ((GameObject)GameObject.Instantiate(
			Orders.instance.prefab,
			Orders.instance.OrderUIContainer.position,
			Quaternion.identity,
			Orders.instance.OrderUIContainer
		)).GetComponent<OrderUI>();
		orderUI.SupplyValues(Items);
	}

	public void Tick() {
		timeLeft -= Time.deltaTime;
		Vector3 scale = orderUI.ProgressBar.transform.localScale;
		scale.x = Mathf.Clamp01(timeLeft / initTime);
		orderUI.ProgressBar.transform.localScale = scale;

		if (timeLeft <= 0) {
			Orders.instance.Fail(this);
			dead = true;
			orderUI.Fail();
		}
	}

	public void Zero() {
		foreach (SushiType st in System.Enum.GetValues(typeof(SushiType))) {
			Items[st] = 0;
		}
	}

	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		foreach (SushiType st in System.Enum.GetValues(typeof(SushiType))) {
			if (Items[st] != 0) {
				sb.Append($"{st}: {Items[st]} ");
			}
		}
		return sb.ToString();
	}

	public bool Check(Dictionary<SushiType, int> sushis) {
		foreach (SushiType st in System.Enum.GetValues(typeof(SushiType))) {
			if (Items[st] != sushis[st]) return false;
		}
		return true;
	}
}

public class Orders : MonoBehaviour {
	public static Orders instance;
	public GameObject prefab;

	List<Order> orders;
	Queue<Order> inactiveOrders;
	Dictionary<SushiType, int> currentlyChecking;

	public Transform OrderUIContainer;

	int maxNumOrders = 1;
	int potentialMaxNumOrders = 5;
	public static float minAmtTime = 45;
	public static float maxAmtTime = 120;
	float minAmtTimeNewMax = 5;
	float maxAmtTimeNewMax = 20;

	int oddsNewOrder = 100;

	AudioSource audioSource;
	[SerializeField] AudioClip soundOrderFailed;
	[SerializeField] AudioClip soundOrderCompleted;

	IEnumerator NewMax() {
		while (maxNumOrders < potentialMaxNumOrders) {
			yield return new WaitForSeconds(Random.Range(minAmtTimeNewMax, maxAmtTimeNewMax));
			maxNumOrders++;
			// Debug.Log($"maxNumOrders is now {maxNumOrders}");
		}
	}

	IEnumerator FailCO(Order order = null) {
		yield return null;
		if (order != null) {
			if (orders.Contains(order)) orders.Remove(order);
			if (!inactiveOrders.Contains(order)) inactiveOrders.Enqueue(order);
		}
		audioSource.PlayOneShot(soundOrderFailed, GameManager.volumeSFX);
		Camera.main.GetComponent<CameraShake>()?.Shake();
		GameManager.IncrementStrikes();
	}

	public void Fail(Order order = null) {
		StartCoroutine(FailCO(order));
	}

	void Start () {
		instance = this;

		orders = new List<Order>();
		orders.Add(new Order());
		// Debug.Log(orders[0].ToString());
		
		inactiveOrders = new Queue<Order>();

		currentlyChecking = new Dictionary<SushiType, int>();

		audioSource = GetComponent<AudioSource>();
		
		StartCoroutine(NewMax());
	}

	void ZeroCurrentlyChecking() {
		foreach (SushiType st in System.Enum.GetValues(typeof(SushiType))) {
			currentlyChecking[st] = 0;
		}
	}

	void Update() {
		if (GameManager.IsGameOver) return;
		foreach (Order order in orders) {
			if (!order.dead)
				order.Tick();
		}
	}
	
	void FixedUpdate () {
		if (GameManager.IsGameOver) return;
		if (orders.Count < maxNumOrders && Random.Range(0, oddsNewOrder) == 0) {
			Order order = null;
			if (inactiveOrders.Count > 0) {
				order = inactiveOrders.Dequeue();
				order.Reset();
			} else {
				order = new Order();
			}

			orders.Add(order);

			// Debug.Log(order.ToString());
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (GameManager.IsGameOver) return;
		if (!col.CompareTag(GameManager.TAG_PLATE)) {
			Fail();
			Destroy(col.gameObject);
			return;
		}

		ZeroCurrentlyChecking();

		Sushi[] sushis = col.GetComponentsInChildren<Sushi>();
		foreach (Sushi sushi in sushis) {
			currentlyChecking[sushi.sushiType]++;
		}

		Order toRemove = null;
		foreach (Order order in orders) {
			if (order.Check(currentlyChecking)) {
				GameManager.IncrementScore();
				toRemove = order;
				break;
			}
		}
		if (toRemove != null) {
			if (orders.Contains(toRemove)) orders.Remove(toRemove);
			if (!inactiveOrders.Contains(toRemove)) inactiveOrders.Enqueue(toRemove);
			audioSource.PlayOneShot(soundOrderCompleted, GameManager.volumeSFX);
			toRemove.orderUI.Succeed();
		} else {
			Fail();
		}

		Destroy(col.gameObject);
	}
}
