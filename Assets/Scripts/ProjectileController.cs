using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

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

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();

		chargeTime = Mathf.Clamp01 (chargeTime);
		float projectileSpeed = Mathf.Lerp (minProjectileSpeed, maxProjectileSpeed, chargeTime);

		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
		direction = new Vector2 (mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

		rb2d.AddForce (direction.normalized * projectileSpeed, ForceMode2D.Impulse);

		Destroy (gameObject, timeAlive);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (obstacles == (obstacles | (1 << other.gameObject.layer))) {
			Destroy (gameObject);
		}

		if (other.CompareTag("Player")) {
			Destroy (gameObject);
		}
	}
}
