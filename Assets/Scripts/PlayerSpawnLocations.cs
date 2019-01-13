using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnLocations : MonoBehaviour {

	public Vector3[] spawnLocations;

	/* Draw waypoints for testing */
	void OnDrawGizmos() {
		if (spawnLocations != null) {
			Gizmos.color = Color.green;
			float size = 0.3f;

			for (int i = 0; i < spawnLocations.Length; i++) {
				//Vector3 globalWaypointPos = (Application.isPlaying)?(globalWaypoints[i]):(localWaypoints [i] + transform.position);
				Gizmos.DrawLine (spawnLocations[i] - Vector3.up * size, spawnLocations[i] + Vector3.up * size);
				Gizmos.DrawLine (spawnLocations[i] - Vector3.left * size, spawnLocations[i] + Vector3.left * size);
			}
		}
	}
}
