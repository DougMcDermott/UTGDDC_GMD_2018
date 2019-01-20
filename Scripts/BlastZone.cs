using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastZone : MonoBehaviour {

	public GameManager gameManager;
	public GameObject playerExplosion;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Body")) {
			if (gameManager.loser == 0) {
				gameManager.loser = other.transform.parent.GetComponent<Player> ().playerIdentity;
				gameManager.waitTime = Time.time + gameManager.timeToNextLevel;
			}
			SoundController.PlaySound ("PlayerKilled");
			Instantiate (playerExplosion, other.transform.position, Quaternion.identity);
			other.transform.parent.gameObject.SetActive (false);
		}
	}
}
