using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	public float upForce = 200f;

	private bool isDead = false;
	private Rigidbody2D rb2d;
	private Animator anim;


	// Start is called before the first frame update
	void Start() {
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update() {

		if (!isDead) {
			if (Input.GetMouseButtonDown(0)) {
				rb2d.velocity = Vector2.zero;
				rb2d.AddForce(new Vector2(0, upForce));
				anim.SetTrigger("Flap");

			}

			// Prevent the bird from flying too high up
			// Makes it easier to pick up the secret coins
			if (transform.position.y >= 20) {
				Vector3 newPos = transform.position;
				newPos.y = 20;
				transform.position = newPos;
			}

		}

	}

	private void OnCollisionEnter2D(Collision2D collision) {
		rb2d.velocity = Vector2.zero;
		isDead = true;
		anim.SetTrigger("Die");
		GameControl.instance.BirdDied();
	}
}
