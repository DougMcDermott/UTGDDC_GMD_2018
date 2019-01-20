using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public Transform shootPoint;
	public Transform playerBody;
	public Transform playerArm;
	public GameObject bulletPrefab;

	Vector2 mousePosition;
	Vector2 direction;
	float angle;
	float timeBetweenShots;
	float currentChargeTime;
	bool facingRight;

	public float rateOfFire;
	public float maxChargeTime;
	public float minProjectileSpeed = 1;
	public float maxProjectileSpeed = 3;

	void Start () {
		
	}

	void LateUpdate () {
//		facingRight = playerBody.GetComponentInParent<Player> ().spriteFacingRight;
//
//		// Calculate the vectors from the player position to the mouse
//		mousePosition = Input.mousePosition;
//		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
//		direction = new Vector2 (mousePosition.x - playerBody.transform.position.x, mousePosition.y - playerBody.transform.position.y);
//		direction.Normalize ();
//
//		// Rotate the gameObject to the direction it is moving
//		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//
//		// First rotate the arm, then translate the arm to the correct position
//		playerArm.transform.rotation = Quaternion.Euler(0, 0, angle);
//		int orientation = (facingRight) ? 1 : -1;
//		playerArm.transform.position = playerBody.position + (new Vector3 (orientation * direction.x * playerBody.localScale.x * 0.5f, direction.y * playerBody.localScale.y * 0.5f, 0));
//
//		if (Input.GetKeyDown(KeyCode.Mouse0)) {
//			currentChargeTime = 0;
//		}
//
//		if (Input.GetKey(KeyCode.Mouse0)) {
//			currentChargeTime += Time.deltaTime;
//		}
//			
//		if (Input.GetKeyUp(KeyCode.Mouse0)) {
//			Fire (direction, timeBetweenShots, currentChargeTime);
//		}
	}
		
	public void Fire(Vector3 direction, float timeBetweenShots, float currentChargeTime) {
		if (Time.time > timeBetweenShots) {
			timeBetweenShots = Time.time + rateOfFire;
			GameObject obj = Instantiate (bulletPrefab, shootPoint.position, Quaternion.identity);
			BulletController bullet = obj.GetComponent<BulletController> ();

			currentChargeTime = Mathf.Clamp01 (currentChargeTime);
			float projectileSpeed = Mathf.Lerp (minProjectileSpeed, maxProjectileSpeed, currentChargeTime);

			bullet.velocity = new Vector3 (direction.normalized.x, direction.normalized.y, 0) * projectileSpeed;

			Destroy (obj, 10);
		}
	}
}
