using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour {


	private float timeSinceStart = 0.0f;
	private bool running = false;

	public void StartTimer() {
		timeSinceStart = 0.0f;
		running = true;
	}

	public void StopTimer() {
		running = false;
	}

	public float getTime() {
		return timeSinceStart;
	}

	void Update() {
		if (running)
			timeSinceStart += Time.deltaTime;
	}



}
