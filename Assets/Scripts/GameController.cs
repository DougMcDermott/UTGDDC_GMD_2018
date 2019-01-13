using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject[] listOfLevels;
	public GameObject[] listOfPlayers;

	int currentLevel;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Q)) {
			BeginGame ();
		}
	}

	void Start () {
		currentLevel = Random.Range (0, listOfLevels.Length);
		BeginGame ();
	}

	void BeginGame () {
		ResetLevel ();
		LoadLevel ();
		SetPlayerPosition ();
	}

	public void ResetLevel () {
		// Destroy all obstacles
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("Level");

		foreach (GameObject current in obstacles) {
			GameObject.Destroy (current); 
		}
			
		// Destroy all players
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject current in players) {
			GameObject.Destroy (current);
		}

		// Destroy all projectiles
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("Projectile");

		foreach (GameObject current in projectiles) {
			GameObject.Destroy (current);
		}
	}

	public void LoadLevel () {
		// Increment counter for next level, change to a random level decider
		currentLevel++;
		currentLevel %= listOfLevels.Length;

		Instantiate (listOfLevels [currentLevel], Vector2.zero, new Quaternion ());
	}

	public void SetPlayerPosition () {
		Vector3[] spawnLocations = listOfLevels [currentLevel].GetComponent<PlayerSpawnLocations> ().spawnLocations;

		for (int i = 0; i < listOfPlayers.Length; i++) {
			Instantiate (listOfPlayers[i], spawnLocations[i], new Quaternion());
		}
	}
}
