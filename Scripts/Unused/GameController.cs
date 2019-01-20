using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {
	
	//[HideInInspector]
	public List<GameObject> listOfPlayers;
	public GameObject[] listOfLevels;

	int currentLevel;

	void Update() {
		if (!hasAuthority) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			BeginGame ();
		}
	}

	void Start () {
		if (!hasAuthority) {
			return;
		}

		currentLevel = Random.Range (0, listOfLevels.Length);
		BeginGame ();
	}

	void BeginGame () {
		CmdResetLevel ();
		CmdLoadLevel ();
		CmdSetPlayerPosition ();
	}

	[Command]
	void CmdResetLevel () {
		// Destroy all obstacles
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("Level");

		foreach (GameObject current in obstacles) {
			GameObject.Destroy (current); 
		}

		// Destroy all projectiles
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("Projectile");

		foreach (GameObject current in projectiles) {
			GameObject.Destroy (current);
		}
	}

	[Command]
	void CmdLoadLevel () {
		// Increment counter for next level, change to a random level decider
		currentLevel++;
		currentLevel %= listOfLevels.Length;

		Instantiate (listOfLevels [currentLevel], Vector2.zero, new Quaternion ());
	}

	[Command]
	void CmdSetPlayerPosition () {
		Vector3[] spawnLocations = listOfLevels [currentLevel].GetComponent<PlayerSpawnLocations> ().spawnLocations;

		int i = 0;
		foreach (GameObject player in listOfPlayers) {
			player.transform.position = spawnLocations [i];
			i++;
		}
	}
}
