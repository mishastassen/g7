using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerContoller : NetworkBehaviour {

	private float speed;
	private float jump;
	private bool hasPackage;
	private bool inRange;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		Eventmanager.Instance.triggerPlayerAdded(this.gameObject);
		hasPackage = false;
	}
	
	void FixedUpdate () {
		if (!isLocalPlayer) { //Check if this is the player corresponding with the local client if not return
			return;
		}
		if (rb == null)
			return;

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float yVelocity = rb.velocity.y;

		if (hasPackage) {
			speed = 3;
			jump = 5;
		} else {
			speed = 5;
			jump = 7;
		}

		if (Input.GetButton("Jump") && (isGroundedToe() || isGroundedHeel())) {
			yVelocity = jump;
		}

		Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);

		rb.velocity = movement;

		if (Input.GetKeyDown (KeyCode.R) && hasPackage) {
			transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
			transform.DetachChildren();
			hasPackage = false;
		}

	}

	bool isGroundedToe() {
		Vector3 toePosition = new Vector3(rb.transform.position.x + 0.5f, rb.transform.position.y, rb.transform.position.z);
		return Physics.Raycast (toePosition, -Vector3.up, 0.1f);
	}

	bool isGroundedHeel() {
		Vector3 heelPosition = new Vector3(rb.transform.position.x - 0.5f, rb.transform.position.y, rb.transform.position.z);
		return Physics.Raycast (heelPosition, -Vector3.up, 0.1f);
	}


    void OnDestroy()
    {
        Eventmanager.Instance.triggerPlayerRemoved(this.gameObject);
    }

	void OnTriggerStay(Collider other) {
		if(Input.GetKeyDown(KeyCode.E) && other.tag == "PickUp" && !hasPackage)
		{	
			other.transform.parent.SetParent(rb.transform);
			other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
			other.transform.parent.localPosition = new Vector3(1,-2,4);
			hasPackage = true;
		}
	}

}
