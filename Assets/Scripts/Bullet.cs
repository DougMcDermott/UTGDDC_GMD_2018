using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

	public float moveSpeed = 10;
	public Vector3 velocity;

	private void FixedUpdate() {
		if (!base.isServer) {
			return;
		}

		transform.position += velocity * Time.deltaTime * moveSpeed;
	}
}
