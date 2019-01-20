using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCursor : MonoBehaviour {

	public Player playerController;
	public WeaponController weaponController;

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("Cancel"))
			TogglePlayerCursor ();
	}

	void TogglePlayerCursor() {
		playerController.enabled = !playerController.enabled;
		weaponController.enabled = !weaponController.enabled;
	}
}
