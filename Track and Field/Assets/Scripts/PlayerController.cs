using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// How many times w should be pressed per second for max speed
	public float keyPressesPerSecond = 9.0f;

	// The maximum speed the player can go
	public float maxSpeed = 10.0f;

	// How much speed should be lost from the targetSpeed each second the user doesn't hit w
	public float speedFalloff = 1.0f;

	// How much speed can be gained each second to match target speed
	// This value should be larger than speedFalloff, so speed can match targetSpeed
	// as it decreases
	public float speedGain = 4.0f;

	// Minimum speed the player must run at or else stand still
	public float speedDampening = 2.5f;

	// The upwards velocity of the player at the start of a jump
	public float jumpVelocity = 7.5f;



	// Camera to follow the player
	public Camera playerCamera;

	// Keeps track of the camera's offset relative to the player
	private Vector3 cameraOffset;



	// Rigid body of the player
	private Rigidbody rb;

	// The actual speed of the player
	private float speed = 0;

	// Speed will slowly be changed to target speed each frame
	private float targetSpeed = 0;



	// Keeps track of time between pressing w
	// Starts at a high number to assume its been forever since
	// the user pressed the button
	private float timeSinceLastWPressed = 1.0f;



	// Keeps track of if the player is running or not
	private bool running = false;

	// Keeps track of if the player is jumping or not
	private bool jumping = false;



	// Allows use of animations
	private Animator anim;



	void Start() {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		cameraOffset = playerCamera.transform.position - transform.position;
	}


	void Update() {

		if (GameController.GetInstance().IsCountdown() && !GameController.GetInstance().IsEnded()) {
			anim.SetTrigger("PrepareTrigger");
		} else {
			anim.ResetTrigger("PrepareTrigger");
		}

		if (GameController.GetInstance().GetWinner() == GameController.Winner.PLAYER) {
			anim.SetTrigger("WinTrigger");
		} else if (GameController.GetInstance().GetWinner() == GameController.Winner.CPU) {
			anim.SetTrigger("LoseTrigger");
		}

		if (GameController.GetInstance().IsEnded() || !GameController.GetInstance().IsStarted()) return;

		if (transform.position.x >= GameController.GetInstance().getGoalPosition()) {

			GameController.GetInstance().PlayerFinished();

			anim.SetTrigger("IdleTrigger");
			rb.velocity = new Vector3(0, 0, 0);
			running = false;
			jumping = false;
			return;

		}

		SetAnimations();
		PerformJump();
		CalculateTargetSpeed();
		ModifySpeed();
		UpdateCameraPos();

	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Ground")) {
			jumping = false;
		} else if (collision.gameObject.CompareTag("Hurdle")) {
			HurdleController hurdle = collision.gameObject.GetComponent<HurdleController>();
			if (!hurdle.HasCollided(gameObject))
				GameController.GetInstance().PlayerPenalty();
			hurdle.ObjectCollided(gameObject);

		}
	}


	// Sets the animation states for running and idling
	private void SetAnimations() {
		anim.SetBool("isRunning", running);
		anim.SetBool("isIdle", !running);
	}

	// Allows the user to jump by pressing D, A, or Space, and sets animations accordingly
	private void PerformJump() {
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space)) {

			if (!jumping) {
				anim.SetTrigger("JumpTrigger");
				jumping = true;
				rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
			}
		}
	}


	// Calculate the target speed based on the rate of pressing w
	// and sets the targetSpeed value accordingly
	private void CalculateTargetSpeed() {
		timeSinceLastWPressed += Time.deltaTime;

		bool wPressed = Input.GetKeyDown(KeyCode.W);
		if (wPressed) {

			targetSpeed = (1.0f / keyPressesPerSecond) / timeSinceLastWPressed * maxSpeed;

			// Prevents hesitation at start
			if (targetSpeed <= speedDampening) targetSpeed = speedDampening + 1f;

			timeSinceLastWPressed = 0;
		} else {
			// Remove some from the target speed
			targetSpeed -= Time.deltaTime * speedFalloff;
		}

		if (targetSpeed < 0) targetSpeed = 0;
		if (targetSpeed > maxSpeed) targetSpeed = maxSpeed;
	}

	// Sets the player's actualy speed based on the target speed, 
	// and changes animations according to the speed
	private void ModifySpeed() {
		float speedDif = targetSpeed - speed; // How much we need to change to match target speed

		if (speedDif < 0) {
			speed -= Time.deltaTime * speedGain;
			if (speed < targetSpeed) speed = targetSpeed;
		} else if (speedDif > 0) {
			speed += Time.deltaTime * speedGain;
			if (speed > targetSpeed) speed = targetSpeed;
		}

		// Speed dampening, so we don't get super slow speeds
		if (speed <= speedDampening && targetSpeed <= speedDampening) speed = 0;

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

	// Update the camera's position based on the player's position
	private void UpdateCameraPos() {
		playerCamera.transform.position = transform.position + cameraOffset;
	}

}
