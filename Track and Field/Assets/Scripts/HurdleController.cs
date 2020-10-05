using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleController : MonoBehaviour {
	// Keeps track of if the player has collided with this hurdle yet
	private Dictionary<GameObject, bool> hitMap;

	void Awake() {
		hitMap = new Dictionary<GameObject, bool>();
	}

	public void ObjectCollided(GameObject obj) {
		if (!hitMap.ContainsKey(obj))
			hitMap.Add(obj, true);
		else
			hitMap[obj] = true;
	}

	public bool HasCollided(GameObject obj) {
		if (!hitMap.ContainsKey(obj)) return false;
		return hitMap[obj];
	}


}
