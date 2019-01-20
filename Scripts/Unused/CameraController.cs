using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Controller target;
	public float verticalOffset;
	public float lookAheadDistanceX;
	public float verticalSmoothTime;
	public float lookSmoothTimeX;
	public Vector2 focusAreaSize;

	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirectionX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	bool lookAheadStop;

	void Start () {
		focusArea = new FocusArea (target.collider.bounds, focusAreaSize);
	}

	void LateUpdate () {
		focusArea.Update (target.collider.bounds);

		Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

		/* Set the look ahead direction and target value
		 * If the player is moving in opposite direction of the target, set the target look ahead to the current + a small fraction */
		if (focusArea.velocity.x != 0) {
			lookAheadDirectionX = Mathf.Sign (focusArea.velocity.x);
			if (Mathf.Sign (target.playerInput.x) == Mathf.Sign (focusArea.velocity.x) && target.playerInput.x != 0) {
				targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
				lookAheadStop = false;
			}
			else {
				if (!lookAheadStop) {
					targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4;
					lookAheadStop = true;
				}
			}
		}

		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

		focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;

		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}

	void OnDrawGizmos () {
		Gizmos.color = new Color (1, 0, 0, 0.5f);
		Gizmos.DrawCube (focusArea.centre, focusAreaSize);
	}

	struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;
		float left, right;
		float top, bot;

		public FocusArea (Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bot = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2 ((left + right)/2, (top + bot)/2);
		}

		public void Update (Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left)
				shiftX = targetBounds.min.x - left;
			else if (targetBounds.max.x > right)
				shiftX = targetBounds.max.x - right;
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bot)
				shiftY = targetBounds.min.y - bot;
			else if (targetBounds.max.y > top)
				shiftY = targetBounds.max.y - top;
			top += shiftY;
			bot += shiftY;

			velocity = new Vector2 (shiftX, shiftY);
			centre = new Vector2 ((left + right)/2, (top + bot)/2);
		}
	};
}
