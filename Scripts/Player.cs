using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	ParticleSystem particles;
	Animator animator;
	SpriteRenderer spriteRenderer;

	public int playerIdentity;
	public string horizontal;
	public string vertical;
	public string jumpButton;

	public Controller controller;

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.4f;
	public float moveSpeed = 6;
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = 0.1f;

	float maxJumpVelocity;
	float minJumpVelocity;
	float gravity;
	float velocityXSmoothing;
	float accelerationTimeAirborne = 0.2f;
	float accelerationTimeGrounded = 0.1f;
	float timeToWallUnstick;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallJumpLeap;
	Vector3 velocity;

	Vector2 input;
	bool jump;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem> ();
		animator = transform.GetChild (0).GetComponent<Animator> ();
		spriteRenderer = transform.GetChild (0).GetComponent<SpriteRenderer> ();

		// From Kinematic Equations
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpHeight = Mathf.Sqrt (2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update () {
		input = new Vector2 (Input.GetAxisRaw(horizontal), Input.GetAxisRaw(vertical));
		if (input.x != 0) {
			animator.SetBool ("isRunning", true);
		} else {
			animator.SetBool ("isRunning", false);
		}

		int wallDirX = (controller.collisions.left)?-1:1;

		if (input.x > 0 && spriteRenderer.flipX)
			Flip ();
		else if (input.x < 0 && !spriteRenderer.flipX)
			Flip ();

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		/* Player is wallsliding if against a wall and moving down
		 * If player input is in opposite direction of wall, create a small delay to may wall leaps more consistent */
		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;
			if (!particles.isPlaying)
				particles.Play ();

			if (velocity.y < -wallSlideSpeedMax)
				velocity.y = -wallSlideSpeedMax;

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (input.x != wallDirX && input.x != 0)
					timeToWallUnstick -= Time.deltaTime;
				else
					timeToWallUnstick = wallStickTime;
			} else {
				timeToWallUnstick = wallStickTime;
			}
		} else if (particles.isPlaying) {
			particles.Stop ();
		}

		//Reset player y velocity if they hit an obstacle
		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;

		// Player jumps. If player is against wall, wall jump based on player input
		jump = false;
		if (Input.GetButtonDown(jumpButton)) {
			if (controller.collisions.below) {
				jump = true;
				velocity.y = maxJumpVelocity;
			} else if (wallSliding) {
				jump = true;
				if (wallDirX == input.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				} else if (input.x == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				} else {
					velocity.x = -wallDirX * wallJumpLeap.x;
					velocity.y = wallJumpLeap.y;
				}
			}
		}

		if (controller.collisions.below) {
			animator.SetBool ("isJumping", false);
		} else {
			animator.SetBool ("isJumping", true);
		}

		// Player jump height varies based on how long they hold the jump button
		if (Input.GetButtonUp (jumpButton)) {
			if (velocity.y > minJumpVelocity)
				velocity.y = minJumpVelocity;
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input, jump);
	}

	//Controls the orientation of the sprite
	void Flip() {
		spriteRenderer.flipX = !spriteRenderer.flipX;
	}
}
