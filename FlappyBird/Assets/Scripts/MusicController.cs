using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

	public static MusicController instance;

	private void Awake() {
		if (instance == null) {
			instance = this;
			// Prevents music from restarting on reload
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}
}
