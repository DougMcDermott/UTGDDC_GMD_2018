using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller))]
public class Player : MonoBehaviour {

	Controller controller;

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.4f;
	public float moveSpeed = 6;
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = 0.1f;

	float maxJumpVelocity;
	float minJumpVelocity;
	[HideInInspector]
	public static float gravity;
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

	[HideInInspector]
	public static bool spriteFacingRight;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller> ();

		spriteFacingRight = true;

		// From Kinematic Equations
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpHeight = Mathf.Sqrt (2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update () {
		input = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		int wallDirX = (controller.collisions.left)?-1:1;

		if (input.x > 0 && !spriteFacingRight)
			Flip ();
		else if (input.x < 0 && spriteFacingRight)
			Flip ();

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		/* Player is wallsliding if against a wall and moving down
		 * If player input is in opposite direction of wall, create a small delay to may wall leaps more consistent */
		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

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
		}

		//Reset player y velocity if they hit an obstacle
		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;

		// Player jumps. If player is against wall, wall jump based on player input
		jump = false;
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
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

		// Player jump height varies based on how long they hold the jump button
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			if (velocity.y > minJumpVelocity)
				velocity.y = minJumpVelocity;
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input, jump);
	}

	//Controls the orientation of the sprite
	void Flip() {
		spriteFacingRight = !spriteFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
