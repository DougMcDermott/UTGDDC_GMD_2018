using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : RaycastController {

	public CollisionInfo collisions;

	[HideInInspector]
	public Vector2 playerInput;
	[HideInInspector]
	public bool playerJump;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		collisions.facingRight = 1;
	}

	public void Move (Vector3 velocity, Vector2 input, bool jump, bool standingOnPlatform = false) {
		UpdateRayCastOrigens ();
		collisions.Reset ();

		playerInput = input;
		playerJump = jump;

		if (velocity.x != 0)
			collisions.facingRight = (int)Mathf.Sign (velocity.x);

		HorizontalCollisions (ref velocity);

		if (velocity.y != 0)
			VerticalCollisions (ref velocity);

		if (standingOnPlatform)
			collisions.below = true;

		transform.Translate (velocity);
	}

	void HorizontalCollisions (ref Vector3 velocity) {
		float directionX = collisions.facingRight;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		if (Mathf.Abs (velocity.x) < skinWidth)
			rayLength = 2 * skinWidth;

		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigen = (directionX == -1) ? raycastOrigens.botLeft : raycastOrigens.botRight;

			rayOrigen += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigen, Vector2.right * directionX, rayLength, collisionMask);

			if (hit) {
				if (hit.distance == 0)
					continue;

				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				if (directionX == -1)
					collisions.left = true;
				else
					collisions.right = true;
			}
		}
	}

	void VerticalCollisions (ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigen = (directionY == -1) ? raycastOrigens.botLeft : raycastOrigens.topLeft;

			rayOrigen += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigen, Vector2.up * directionY, rayLength, collisionMask);

			if (hit) {
				if (hit.collider.CompareTag ("Through")) {
					if (directionY == 1)
						continue;
				}

				if (hit.distance == 0)
					continue;

				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (directionY == -1)
					collisions.below = true;
				else
					collisions.above = true;
			}
		}
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;
		public int facingRight;

		public void Reset () {
			above = false;
			below = false;
			left = false;
			right = false;
		}
	};
}
