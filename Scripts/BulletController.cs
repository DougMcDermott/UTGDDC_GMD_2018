using UnityEngine;

public class BulletController : MonoBehaviour {

	GameManager gameManager;

	public LayerMask obstacles;
	public LayerMask player; 

	public GameObject explosion;
	public GameObject playerExplosion;

	public float moveSpeed = 10;
	public float gravity = -1;
	public Vector3 velocity;

	void Start() {
		gameManager = FindObjectOfType<GameManager> ();
	}

	void Update() {
		velocity.y += (gravity) * Time.deltaTime;
		transform.Translate (velocity * moveSpeed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (obstacles == (obstacles | (1 << other.gameObject.layer))) {
			SoundController.PlaySound ("ProjectileDestroyed");
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}

		if (other.CompareTag("Body")) {
			if (gameManager.loser == 0) {
				SoundController.PlaySound ("PlayerKilled");
				gameManager.loser = other.transform.parent.GetComponent<Player> ().playerIdentity;
				gameManager.waitTime = Time.time + gameManager.timeToNextLevel;

				Instantiate (playerExplosion, transform.position, Quaternion.identity);
				other.transform.parent.gameObject.SetActive(false);
			}
			Destroy (gameObject);
		}
	}
}
