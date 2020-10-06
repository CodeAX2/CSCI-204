using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

	// Singleton pattern
	private static GameController instance;



	// Enum to describe winner
	public enum Winner { NONE, PLAYER, CPU };



	// Refrences to the timer of the game
	private TimerController playerTimerController;
	private TimerController cpuTimerController;

	// Refrences to the text that displays the timer
	public TextMeshProUGUI playerTimerText;
	public TextMeshProUGUI cpuTimerText;



	// Keeps track of penalties for hitting the hurdles
	private float playerPenalty = 0;
	private float cpuPenalty = 0;

	// Refrences to the text that displays the penalty
	public TextMeshProUGUI playerPenaltyText;
	public TextMeshProUGUI cpuPenaltyText;



	// Refrence to the text that displays the countdown
	public TextMeshProUGUI countdownText;



	// Keeps track of the goal
	public GameObject goal;



	// Keeps track of the countdown to start the game
	private float countdownTime = 3.0f;

	// Keeps track of if the countdown has begun yet
	private bool countdownStarted = false;



	// Keeps track of if the game has started
	// Game starts when the w key is pressed for the first time
	private bool started = false;

	// Keeps track of when the game has ended
	private bool ended = false;

	// Keeps track of if the user and cpu have finished
	private bool playerFinished = false, cpuFinished = false;

	void Awake() {
		// Singleton pattern
		if (instance == null) instance = this;
		else Destroy(gameObject);
	}

	void Start() {
		playerTimerController = gameObject.AddComponent<TimerController>();
		cpuTimerController = gameObject.AddComponent<TimerController>();
	}


	void Update() {

		if (Input.GetKeyDown(KeyCode.W) && !started && !countdownStarted) {
			StartCountdown();
		}

		if (countdownStarted)
			PerformCountdown();

		if (IsStarted()) {
			UpdateText();
		}

	}


	private void StartCountdown() {
		countdownStarted = true;
	}

	private void PerformCountdown() {
		countdownTime -= Time.deltaTime;

		UpdateCountdownText();

		if (countdownTime <= 0.0f && !started) {
			StartGame();
		}
	}

	private void UpdateCountdownText() {
		int countdownInt = (int)Mathf.Ceil(countdownTime);
		if (countdownInt > 0) {
			countdownText.text = countdownInt.ToString();
		} else if (countdownInt > -1) {
			countdownText.text = "GO!";
		} else {
			countdownText.text = "";
			countdownStarted = false;
		}
	}

	private void StartGame() {
		started = true;
		playerTimerController.StartTimer();
		cpuTimerController.StartTimer();
	}

	// Update the timer and penalty texts
	private void UpdateText() {

		playerTimerText.text = playerTimerController.getTime().ToString("0.00") + "s";
		if (playerPenalty != 0) {
			playerPenaltyText.text = "+" + playerPenalty.ToString("0.00") + "s";
		}

		cpuTimerText.text = cpuTimerController.getTime().ToString("0.00") + "s";
		if (cpuPenalty != 0) {
			cpuPenaltyText.text = "+" + cpuPenalty.ToString("0.00") + "s";
		}
	}

	// If the countdown is occuring
	public bool IsCountdown() {
		return countdownStarted && !started;
	}

	// If the game has started
	public bool IsStarted() {
		return started;
	}

	// If the game has ended
	public bool IsEnded() {
		return ended;
	}

	public void PlayerFinished() {
		playerTimerController.StopTimer();
		playerFinished = true;
		if (cpuFinished)
			ended = true;
	}

	public void CPUFinished() {
		cpuTimerController.StopTimer();
		cpuFinished = true;
		if (playerFinished)
			ended = true;
	}

	public void PlayerPenalty() {
		playerPenalty += 0.5f;
	}

	public void CPUPenalty() {
		cpuPenalty += 0.5f;
	}

	public float getGoalPosition() {
		return goal.transform.position.x;
	}

	public Winner GetWinner() {
		if (!ended) return Winner.NONE;

		float playerTotal = playerTimerController.getTime() + playerPenalty;
		float cpuTotal = cpuTimerController.getTime() + cpuPenalty;

		if (playerTotal <= cpuTotal) return Winner.PLAYER;
		else return Winner.CPU;


	}

	// Get the single instance of the GameController
	public static GameController GetInstance() {
		return instance;
	}

}
