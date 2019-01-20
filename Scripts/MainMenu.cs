using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject sprite1;
	public GameObject sprite2;

	public void PlayGame () {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void SetActive () {
		sprite1.SetActive (!sprite1.activeSelf);
		sprite2.SetActive (!sprite2.activeSelf);
	}

	public void QuitGame () {
		Debug.Log ("Quit");
		Application.Quit ();
	}

}
