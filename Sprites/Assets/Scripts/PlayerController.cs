using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private bool grounded, jump, running;

	private Animator anim;
	private Rigidbody2D rb;

	public float power = 1.0f, speed = 1.0f;

	private void Awake() {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}


	// Start is called before the first frame update
	void Start() {
		grounded = false;
		jump = false;
	}

	// Update is called once per frame
	void Update() {

		if (Input.GetButtonDown("Jump")) {
			if (grounded) {
				anim.SetTrigger("jumpTrigger");
				grounded = false;
				jump = true;
			}

		} else if (Input.GetButtonDown("Horizontal")) {
			anim.SetTrigger("runTrigger");
			jump = false;
			running = true;
		} else if (grounded && !Input.GetButtonDown("Horizontal")) {
			anim.SetTrigger("idleTrigger");
			running = false;
		}


	}

	private void FixedUpdate() {
		if (jump) {
			rb.AddForce(new Vector2(0, power));
			jump = false;
		}

		if (running) {
			rb.velocity = new Vector2(speed, rb.velocity.y);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		grounded = true;
	}
}
