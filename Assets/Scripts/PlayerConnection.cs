using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

	public GameObject playerPrefab;

	void Start () {
		if (!isLocalPlayer) {
			return;
		}

		CmdSpawnPlayerUnit ();
	}

	void Update () {
		
	}

	/* Commands */
	[Command]
	void CmdSpawnPlayerUnit () {
		GameObject player = Instantiate (playerPrefab);

		NetworkServer.SpawnWithClientAuthority (player, connectionToClient);
	}
}
