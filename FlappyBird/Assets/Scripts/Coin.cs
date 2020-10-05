using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public AudioClip pickupSound;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<Bird>() != null) {
			// Play a soung on coin pickup, incriment score, and remove the coin
			AudioSource.PlayClipAtPoint(pickupSound, transform.position);
			GameControl.instance.BirdScored();
			Destroy(gameObject);
		}
	}


}
