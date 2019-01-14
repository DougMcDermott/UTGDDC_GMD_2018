using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour {

	public GameObject projectile;
	public Transform shootPoint;

	public Transform playerBody;
	public Transform playerArm;

	Vector2 mousePosition;
	Vector2 direction;
	float angle;
	float timeBetweenShots;
	float currentChargeTime;
	bool facingRight;

	public float rateOfFire;
	public float maxChargeTime;

	void Start () {
		
	}

	void LateUpdate () {
		if (!isLocalPlayer) {
			return;
		}

		facingRight = Player.spriteFacingRight;

		// Calculate the vectors from the player position to the mouse
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
		direction = new Vector2 (mousePosition.x - playerBody.transform.position.x, mousePosition.y - playerBody.transform.position.y);
		direction.Normalize ();

		// Rotate the gameObject to the direction it is moving
		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		// First rotate the arm, then translate the arm to the correct position
		playerArm.transform.rotation = Quaternion.Euler(0, 0, angle);
		int orientation = (facingRight) ? 1 : -1;
		playerArm.transform.position = playerBody.position + (new Vector3 (orientation * direction.x * playerBody.localScale.x * 0.5f, direction.y * playerBody.localScale.y * 0.5f, 0));

//		if (Input.GetKeyDown(KeyCode.Mouse0)) {
//			currentChargeTime = 0;
//		}
//
//		if (Input.GetKey(KeyCode.Mouse0)) {
//			currentChargeTime += Time.deltaTime;
//		}
//			
//		if (Input.GetKeyUp(KeyCode.Mouse0)) {
//			CmdFireProjectile ();
//		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			CmdPrintTest ("Client:" + mousePosition);

			CmdShoot (direction);
		}
	}

	public GameObject bulletPrefab;

	[Command]
	public void CmdShoot(Vector3 direction) {
		GameObject obj = Instantiate (bulletPrefab, shootPoint.position, Quaternion.identity);
		Bullet bullet = obj.GetComponent<Bullet> ();
		bullet.velocity = new Vector3(direction.normalized.x, direction.normalized.y, 0);

		print ("Server:" + Camera.main.ScreenToWorldPoint(Input.mousePosition));

		Destroy (obj, 10);
		NetworkServer.Spawn (obj);
	}

	[Command]
	public void CmdPrintTest(string test) {
		print (test);
	}

//	public float minProjectileSpeed = 10;
//	public float maxProjectileSpeed = 30;
//
//	//instantiate a new bullet
//	[Command]
//	void CmdFireProjectile () {
//		if (Time.time > timeBetweenShots) {
////			timeBetweenShots = Time.time + rateOfFire;
////			GameObject obj =  (GameObject)Instantiate (projectile, shootPoint.position, new Quaternion());
////			//obj.GetComponent<ProjectileController> ().chargeTime = currentChargeTime;
////
////			currentChargeTime = Mathf.Clamp01 (currentChargeTime);
////			float projectileSpeed = Mathf.Lerp (minProjectileSpeed, maxProjectileSpeed, currentChargeTime);
////
////			Rigidbody2D rb2d = obj.GetComponent<Rigidbody2D> ();
////			rb2d.velocity = (direction.normalized * projectileSpeed);
////			//rb2d.AddForce (direction.normalized * projectileSpeed, ForceMode2D.Impulse);
////
////			//Debug.Log ("Force added to the projectile: " + (direction.normalized * projectileSpeed));
////
////			NetworkServer.Spawn (obj);
//
//			print ("CmdFireProjectile");
//
//			timeBetweenShots = Time.time + rateOfFire;
//			GameObject obj =  (GameObject)Instantiate (projectile, shootPoint.position, new Quaternion());
//			obj.GetComponent<ProjectileController> ().chargeTime = currentChargeTime;
//		}
//	}
}
