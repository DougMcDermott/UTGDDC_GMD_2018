using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public static AudioClip arrow;
	public static AudioClip magic;
	public static AudioClip playerKilled;
	public static AudioClip projectileDestroyed;


	static AudioSource audioSource;

	// Use this for initialization
	void Start () {
		arrow = Resources.Load<AudioClip> ("Arrow");
		magic = Resources.Load<AudioClip> ("Magic");
		playerKilled = Resources.Load<AudioClip> ("PlayerKilled");
		projectileDestroyed = Resources.Load<AudioClip> ("ProjectileDestroyed");

		audioSource = GetComponent<AudioSource> ();
	}

	public static void PlaySound (string clip) {
		switch (clip) {
		case "Arrow":
			audioSource.PlayOneShot (arrow);
			break;
		case "Magic":
			audioSource.PlayOneShot (magic);
			break;
		case "PlayerKilled":
			audioSource.PlayOneShot (playerKilled);
			break;
		case "ProjectileDestroyed":
			audioSource.PlayOneShot (projectileDestroyed);
			break;
		}
	}
}
