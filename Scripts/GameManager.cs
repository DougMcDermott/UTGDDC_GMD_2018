using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject[] listOfLevels;
	public GameObject[] players;

	[HideInInspector]
	public int loser;

	int currentLevel;

	[HideInInspector]
	public float waitTime;
	public float timeToNextLevel = 3;

	void Start () {
		int currentLevel = 0;
		loser = 0;
		BeginGame ();
	}

	void Update () {
		if (Input.GetButton ("Cancel")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex - 1);
		}

		if (loser != 0) {
			if (Time.time > waitTime) {
				BeginGame ();
			}
		}
	}

	public void BeginGame () {
		loser = 0;
		ResetLevel ();
		LoadLevel (currentLevel);
		SetPlayerPosition (currentLevel);

		// Increment counter for next level, change to a random level decider
		currentLevel++;
		currentLevel %= listOfLevels.Length;
	}

	void ResetLevel () {
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

		// Destroy all explosions
		GameObject[] explosions = GameObject.FindGameObjectsWithTag ("Explosion");

		foreach (GameObject current in explosions) {
			GameObject.Destroy (current);
		}
	}

	void LoadLevel (int level) {
		GameObject obj = Instantiate (listOfLevels [level], Vector2.zero, new Quaternion ());
	}

	void SetPlayerPosition (int level) {
		Vector3[] spawnLocations = listOfLevels [level].GetComponent<PlayerSpawnLocations> ().spawnLocations;

		int i = 0;
		foreach (GameObject current in players) { 
			current.SetActive(true);
			current.transform.position = spawnLocations [i];
			i++;
		}
	}
}
