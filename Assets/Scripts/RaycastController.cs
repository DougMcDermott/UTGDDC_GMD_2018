using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RaycastController : NetworkBehaviour {

	public LayerMask collisionMask;

	public const float skinWidth = .015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	[HideInInspector]
	public float horizontalRaySpacing;
	[HideInInspector]
	public float verticalRaySpacing;

	public BoxCollider2D collider;
	public RaycastOrigins raycastOrigens;

	// Needs to be awake because it is called before start, eliminates errors with Camera Controller
	public virtual void Awake() {
		if (!transform.CompareTag("Player"))
			collider = GetComponent<BoxCollider2D> ();

		if (collider == null)
			Debug.Log ("BoxCollider2D missing");
	}

	public virtual void Start () {
		CalculateRaySpacing ();
	}

	public void UpdateRayCastOrigens() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigens.botLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigens.botRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigens.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigens.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	public void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 botLeft, botRight;
	}
}