using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public GameObject projectile;
	public Transform shootPoint;

	Transform playerBody;

	Vector2 mousePosition;
	Vector2 direction;
	float angle;
	float timeBetweenShots;
	float currentChargeTime;
	bool facingRight;

	public float rateOfFire;
	public float maxChargeTime;

	void Start () {
		playerBody = transform.parent.GetChild (0);
	}

	void LateUpdate () {
		facingRight = Player.spriteFacingRight;

		// Calculate the vectors from the player position to the mouse
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
		direction = new Vector2 (mousePosition.x - playerBody.transform.position.x, mousePosition.y - playerBody.transform.position.y);
		direction.Normalize ();

		// Rotate the gameObject to the direction it is moving
		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		// First rotate the arm, then translate the arm to the correct position
		transform.rotation = Quaternion.Euler(0, 0, angle);
		int orientation = (facingRight) ? 1 : -1;
		transform.position = playerBody.position + (new Vector3 (orientation * direction.x * playerBody.localScale.x * 0.5f, direction.y * playerBody.localScale.y * 0.5f, 0));

		if (Input.GetKeyDown (KeyCode.Space)) {
			currentChargeTime = 0;
		}

		if (Input.GetKey(KeyCode.Space)) {
			currentChargeTime += Time.deltaTime;
		}
			
		if (Input.GetKeyUp(KeyCode.Space)) {
			if (currentChargeTime <= maxChargeTime)
				fireProjectile ();
		}
	}

	//instantiate a new bullet
	void fireProjectile () {
		if (Time.time > timeBetweenShots) {
			timeBetweenShots = Time.time + rateOfFire;
			GameObject obj =  (GameObject)Instantiate (projectile, shootPoint.position, new Quaternion());
			obj.GetComponent<ProjectileController> ().chargeTime = currentChargeTime;
		}
	}
}
