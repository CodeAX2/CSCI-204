using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public float rotateSpeedX = 0, rotateSpeedY = 0, rotateSpeedZ = 0;

	void Update() {

		transform.Rotate(new Vector3(rotateSpeedX, rotateSpeedY, rotateSpeedZ) * Time.deltaTime);
		// I modified rotator to make it more generic and therefore more useful in a variety of situations

	}
}
