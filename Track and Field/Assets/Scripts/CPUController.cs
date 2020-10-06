using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUController : MonoBehaviour {

	// The upwards velocity of the computer at the start of a jump
	public float jumpVelocity = 7.5f;

	// The actual speed of the computer
	public float speed = 9.0f;



	// Rigid body of the computer
	private Rigidbody rb;



	// Keeps track of if the player is running or not
	private bool running = false;

	// Keeps track of if the player is jumping or not
	private bool jumping = false;



	// Allows use of animations
	private Animator anim;



	void Start() {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}


	void Update() {

		if (GameController.GetInstance().IsCountdown() && !GameController.GetInstance().IsEnded()) {
			anim.SetTrigger("PrepareTrigger");
		} else {
			anim.ResetTrigger("PrepareTrigger");
		}

		if (GameController.GetInstance().GetWinner() == GameController.Winner.CPU) {
			anim.SetTrigger("WinTrigger");
		} else if (GameController.GetInstance().GetWinner() == GameController.Winner.PLAYER) {
			anim.SetTrigger("LoseTrigger");
		}

		if (GameController.GetInstance().IsEnded() || !GameController.GetInstance().IsStarted()) return;

		if (transform.position.x >= GameController.GetInstance().getGoalPosition()) {

			GameController.GetInstance().CPUFinished();

			anim.SetTrigger("IdleTrigger");
			rb.velocity = new Vector3(0, 0, 0);
			running = false;
			jumping = false;
			return;

		}

		SetAnimations();
		ModifySpeed();

	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Ground")) {
			jumping = false;
		} else if (collision.gameObject.CompareTag("Hurdle")) {
			HurdleController hurdle = collision.gameObject.GetComponent<HurdleController>();
			if (!hurdle.HasCollided(gameObject))
				GameController.GetInstance().CPUPenalty();
			hurdle.ObjectCollided(gameObject);

		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("CPUHurdleTrigger")) {
			PerformJump();
		}
	}

	// Sets the animation states for running and idling
	private void SetAnimations() {
		anim.SetBool("isRunning", running);
		anim.SetBool("isIdle", !running);
	}

	// Jumps the computer
	private void PerformJump() {

		if (!jumping) {
			anim.SetTrigger("JumpTrigger");
			jumping = true;
			rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
		}

	}


	// Sets the cpu's speed and changes animations accordingly
	private void ModifySpeed() {

		bool wasRunning = running; // Keep track of if we were running before
								   // Update our running
		if (speed <= 0) {
			running = false;
		} else {
			running = true;
		}

		if (!wasRunning && running) {
			anim.SetTrigger("RunTrigger");
		} else if (wasRunning && !running) {
			anim.SetTrigger("IdleTrigger");
		}

		rb.velocity = new Vector3(speed, rb.velocity.y, 0);
	}

}
