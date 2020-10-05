using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {

	public float speed = 0;
	public TextMeshProUGUI countText;
	public TextMeshProUGUI winTextObject;

	private Rigidbody rb;
	private float movementX, movementY;

	private int count;

	void Start() {
		rb = GetComponent<Rigidbody>();
		count = 0;

		SetCountText();
		winTextObject.gameObject.SetActive(false);
	}

	void OnMove(InputValue movementValue) {
		Vector2 movementVector = movementValue.Get<Vector2>();
		movementX = movementVector.x;
		movementY = movementVector.y;
	}

	void SetCountText() {
		countText.text = "Count: " + count.ToString();
		if (count >= 12) {
			winTextObject.gameObject.SetActive(true);
		}
	}

	void FixedUpdate() {
		Vector3 movement = new Vector3(movementX, 0.0f, movementY);
		rb.AddForce(movement * speed);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("PickUp")) {
			other.gameObject.SetActive(false);
			count++;
			SetCountText();
		} else if (other.CompareTag("LoseDetector")) {
			winTextObject.text = "You Lose";
			winTextObject.gameObject.SetActive(true);
		}
	}

}
