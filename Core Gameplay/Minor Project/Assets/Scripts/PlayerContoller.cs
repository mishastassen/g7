using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerContoller : NetworkBehaviour {

	public float speed;
	public float jump;
	private Rigidbody rb;
	private bool isGrounded;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
		if (!isLocalPlayer) { //Check if this is the player corresponding with the local client if not return
			return;
		}
		if (rb == null)
			return;
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float yVelocity = rb.velocity.y;

		if (Input.GetButton("Fire1") && isGrounded) {
			yVelocity = jump;
		}
		
		Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);

		rb.velocity = movement;
	}

	void OnCollisionStay(Collision c) {
		isGrounded = true;
	}

	void OnCollisionExit(Collision c) {
		isGrounded = false;
	}

	/**
	bool isGrounded() {
		return Physics.Raycast (rb.transform.position, -Vector3.up, 0.1f);
	}
	*/
}
