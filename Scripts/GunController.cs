using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController: MonoBehaviour {

	public string weapon;
	public string rotate;
	public string fire;

	public Transform shootPoint;
	public Transform playerBody;
	public Transform playerArm;
	public GameObject bulletPrefab;

	Vector2 direction;
	float angle;
	float timeBetweenShots;
	float currentChargeTime;

	public float speed;

	public float rateOfFire;
	public float maxChargeTime;
	public float minProjectileSpeed = 1;
	public float maxProjectileSpeed = 3;

	void Start () {
		angle = transform.rotation.z;
	}

	void LateUpdate () {
		float input = Input.GetAxisRaw (rotate);
		angle += input * speed * Time.deltaTime;

		direction = (Vector2)(Quaternion.Euler(0,0,angle) * Vector2.right);

		// First rotate the arm, then translate the arm to the correct position
		playerArm.transform.rotation = Quaternion.Euler(0, 0, angle);
		playerArm.transform.position = playerBody.position + (new Vector3 (direction.x * playerBody.localScale.x * .7f, direction.y * playerBody.localScale.y * .7f, -1));

		if (Input.GetButtonDown(fire)) {
			currentChargeTime = 0;
		}

		if (Input.GetButton(fire)) {
			currentChargeTime += Time.deltaTime;
		}

		if (Input.GetButtonUp(fire)) {
			Fire ();
		}
	}

	public void Fire() {
		if (Time.time > timeBetweenShots) {
			SoundController.PlaySound (weapon);
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
