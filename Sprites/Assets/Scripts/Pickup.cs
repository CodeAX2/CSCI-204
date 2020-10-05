using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

	private int coinCount;
	public Text scoreText;
	public AudioClip coinSound;

	private void OnTriggerEnter2D(Collider2D collision) {

		if (collision.CompareTag("Coin")) {
			coinCount++;
			AudioSource.PlayClipAtPoint(coinSound, collision.transform.position);
			Destroy(collision.gameObject);
			scoreText.text = coinCount.ToString();

		}


	}


}
