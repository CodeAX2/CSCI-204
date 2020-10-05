using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPool : MonoBehaviour {

	public static ColumnPool instance;

	public int columnPoolSize = 20;
	public GameObject columnPrefab;
	public float columnMin = -1f, columnMax = 3.5f;

	public float[] spawnRates; // Allows for a changing difficulty
	private int difficulty = 0; // Keeps track of the current difficulty
	private int numSpawned = 0; // Keeps track of how many pillars have been spawned

	private GameObject[] columns;
	private Vector2 objectPoolPosition = new Vector2(-15f, -25f);
	private float timeSinceLastSpawned;
	private float spawnXPosition = 10f;
	private int currentColumn = 0;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		if (spawnRates.Length == 0) {
			spawnRates = new float[1] { 4f };
		}

	}

	// Start is called before the first frame update
	void Start() {
		columns = new GameObject[columnPoolSize];

		for (int i = 0; i < columnPoolSize; i++) {
			columns[i] = Instantiate(columnPrefab, objectPoolPosition, Quaternion.identity);
		}
		timeSinceLastSpawned = spawnRates[difficulty] - 1f;

	}

	// Update is called once per frame
	void Update() {
		timeSinceLastSpawned += Time.deltaTime;
		if (!GameControl.instance.gameOver && timeSinceLastSpawned >= spawnRates[difficulty]) {
			timeSinceLastSpawned = 0;

			float spawnYPosition = Random.Range(columnMin, columnMax);

			columns[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);
			currentColumn++;

			if (currentColumn >= columnPoolSize) {
				currentColumn = 0;
			}

			numSpawned++; // Keeps track of how many pillars we have put ahead of the player

			// Every 5 pillars, increase the difficulty
			if (numSpawned % 5 == 0) {
				difficulty++;
				if (difficulty >= spawnRates.Length) difficulty = spawnRates.Length - 1;
			}

		}
	}
}
