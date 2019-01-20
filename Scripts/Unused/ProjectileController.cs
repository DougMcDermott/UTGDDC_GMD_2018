using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileController : NetworkBehaviour {

	public LayerMask obstacles;
	public LayerMask player;

	Rigidbody2D rb2d;

	Vector2 mousePosition;
	Vector2 direction;

	[HideInInspector]
	public float chargeTime;
	public float minProjectileSpeed;
	public float maxProjectileSpeed;
	public float timeAlive;

//	public Vector2 shootDir;
//	public float projectileSpeed;
//
//	public override void OnStartServer() {
//		if (!base.isServer)
//			return;
//
//		rb2d = GetComponent<Rigidbody2D> ();
//		rb2d.AddForce (direction.normalized * projectileSpeed, ForceMode2D.Impulse); 
//	}

//	private void Start () {
//		if (!base.isServer) {
//			return;
//		}
//
//		rb2d = GetComponent<Rigidbody2D> ();
//
//		chargeTime = Mathf.Clamp01 (chargeTime);
//		float projectileSpeed = Mathf.Lerp (minProjectileSpeed, maxProjectileSpeed, chargeTime);
//
//		mousePosition = Input.mousePosition;
//		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
//		direction = new Vector2 (mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
//
//		//Debug.DrawLine (transform.position, mousePosition);
//
//		Debug.DrawRay (transform.position, direction.normalized * projectileSpeed);
//
//		print ("ProjectileController");
//
//		rb2d.AddForce (direction.normalized * projectileSpeed, ForceMode2D.Impulse); 
//		//CmdShootProjectile(projectileSpeed);
//	}

//	[Command]
//	void CmdShootProjectile (float projectileSpeed) {
//		rb2d.AddForce (direction.normalized * projectileSpeed, ForceMode2D.Impulse);
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!base.isServer)
			return;

		if (obstacles == (obstacles | (1 << other.gameObject.layer))) {
			NetworkServer.Destroy (gameObject);
		}

		if (other.CompareTag("Player")) {
			NetworkServer.Destroy (gameObject);
		}
	}
}
