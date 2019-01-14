using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ToggleCursor : NetworkBehaviour {

	public Player playerController;
	public WeaponController weaponController;

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;

		if (Input.GetButtonUp ("Cancel"))
			TogglePlayerCursor ();
	}

	void TogglePlayerCursor() {
		playerController.enabled = !playerController.enabled;
		weaponController.enabled = !weaponController.enabled;
	}
}
