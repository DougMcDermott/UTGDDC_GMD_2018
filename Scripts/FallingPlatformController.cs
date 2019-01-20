using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : RaycastController {

	public LayerMask passengerMask;

	public float platformFallTime = 1;
	public float delayAfterFall = 1;
	public float gravity = -1;

	bool platformTriggered;
	float timeToPlatformFall;
	Vector3 velocity;

	List<PassengerMovement> passengerMovement;
	Dictionary<Transform, Controller> passengerDictionary = new Dictionary<Transform, Controller> ();

	public override void Start () {
		base.Start ();

		platformTriggered = false;
		velocity = Vector2.zero;
	}

	void Update () {
		UpdateRayCastOrigens ();

		DetectPassenger ();
		if (platformTriggered) {
			if (timeToPlatformFall <= 0) {
				velocity.y += gravity * Time.deltaTime;
			} else {
				timeToPlatformFall -= Time.deltaTime;
			}
		}

		transform.Translate (velocity * Time.deltaTime);
		CalculatePassengerMovement (velocity);
		foreach (PassengerMovement passenger in passengerMovement) {
			if (!passengerDictionary.ContainsKey(passenger.transform))
				passengerDictionary.Add (passenger.transform, passenger.transform.parent.GetComponent<Controller> ());
			passengerDictionary[passenger.transform].Move (passenger.velocity, Vector2.zero, false, passenger.standingOnPlatform);
		}
	}

	void DetectPassenger () {
		float rayLength = skinWidth * 2;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigen = raycastOrigens.topLeft;

			rayOrigen += Vector2.right * (verticalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigen, Vector2.up, rayLength, passengerMask);

			if (hit) {
				if (!platformTriggered) {
					Destroy (gameObject, delayAfterFall + platformFallTime);
					timeToPlatformFall = platformFallTime;
				}
				platformTriggered = true;
				break;
			}
		}

	}

	void CalculatePassengerMovement (Vector3 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement = new List<PassengerMovement> ();

		// Passenger on top of downward moving platform
		if (velocity.y != 0) {
			float rayLength = skinWidth * 2;

			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigen = raycastOrigens.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast (rayOrigen, Vector2.up, rayLength, passengerMask);

				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add (hit.transform);

						float pushX = velocity.x;
						float pushY = velocity.y;

						// Passenger is on platform, always want platform to move first
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
					}
				}
			}
		}
	}

	struct PassengerMovement {
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement (Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	};
}
