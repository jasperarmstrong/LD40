using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SushiSprite {
	public SushiType sushiType;
	public Sprite sprite;
}

public enum SushiType {
	WHITE, PINK, ORANGE, GREEN
}

public class Sushi : MonoBehaviour {
	public SushiType sushiType;
}
