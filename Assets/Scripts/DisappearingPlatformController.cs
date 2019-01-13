using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatformController : RaycastController {

	public Controller controller;
	MeshRenderer meshRenderer;
	BoxCollider2D boxCollider2D;

	public bool startEnabled;

	public override void Start () {
		base.Start ();

		meshRenderer = GetComponent<MeshRenderer> ();
		boxCollider2D = GetComponent<BoxCollider2D> ();

		if (!startEnabled) {
			meshRenderer.enabled = false;
			boxCollider2D.enabled = false;
		}
	}

	void Update () {
		if (controller.playerJump) {
			meshRenderer.enabled = !meshRenderer.enabled;
			boxCollider2D.enabled = !boxCollider2D.enabled;
		}
	}
}
