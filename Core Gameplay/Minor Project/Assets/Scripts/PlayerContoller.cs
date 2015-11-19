using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerContoller : NetworkBehaviour {

	private float speed;
	private float jump;

	[SyncVar]
	private bool hasPackage;

	private Rigidbody rb;

	private float fastspeed;
	private float fastjump;
	private float slowspeed;
	private float slowjump;

	void Start () {
		rb = GetComponent<Rigidbody>();
		Eventmanager.Instance.triggerPlayerAdded(this.gameObject);
		hasPackage = false;
		fastspeed = 10;
		fastjump = 20;
		slowspeed = 6;
		slowjump = 15;
	}
	
	void FixedUpdate () {
		if (!isLocalPlayer) { //Check if this is the player corresponding with the local client if not return
			return;
		}
		if (rb == null)
			return;

		// move player based on user input
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float yVelocity = rb.velocity.y;

		// set speed and jumppower
		if (hasPackage) {
			speed = slowspeed;
			jump = slowjump;
		} else {
			speed = fastspeed;
			jump = fastjump;
		}

		// jump based on user input
		if (Input.GetButton("Jump") && (isGroundedToe() || isGroundedHeel())) {
			yVelocity = jump;
		}

		// move player
		Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);
		rb.velocity = movement;

		// drop the package
		if (Input.GetKeyDown (KeyCode.R) && hasPackage) {
			transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
			transform.DetachChildren();
			hasPackage = false;
			CmdDropPackage();
		}

		// throw a package
		if (Input.GetKeyDown (KeyCode.T) && hasPackage) {
			transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
			transform.GetChild(0).GetComponent<Rigidbody>().AddForce(new Vector3(5000,5000,0));
			transform.DetachChildren();			
			hasPackage = false;
			CmdThrowPackage();
		}

	}

	// checks whether the front of the player is on a platform
	bool isGroundedToe() {
		Vector3 toePosition = new Vector3(rb.transform.position.x + 0.5f, rb.transform.position.y, rb.transform.position.z);
		return Physics.Raycast (toePosition, -Vector3.up, 0.1f);
	}

	// checks whether the back of the player is on a platform
	bool isGroundedHeel() {
		Vector3 heelPosition = new Vector3(rb.transform.position.x - 0.5f, rb.transform.position.y, rb.transform.position.z);
		return Physics.Raycast (heelPosition, -Vector3.up, 0.1f);
	}


    void OnDestroy()
    {
        Eventmanager.Instance.triggerPlayerRemoved(this.gameObject);
    }

	// pick up or catch a package
	void OnTriggerStay(Collider other) {
		if(Input.GetKeyDown(KeyCode.E) && other.tag == "PickUp1" && !hasPackage)
		{	
			other.transform.parent.SetParent(rb.transform);
			other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
			other.transform.parent.localPosition = new Vector3(1,-2,4);
			CmdPickupPackage("PickUp1");
			hasPackage = true;
		}
	}

	[Command]
	void CmdPickupPackage(string tag){
		GameObject other = GameObject.FindWithTag(tag);
		other.transform.parent.SetParent(rb.transform);
		other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
		other.transform.parent.localPosition = new Vector3(1,-2,4);
	}

	[Command]
	void CmdDropPackage(){
		transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
		transform.DetachChildren();
	}

	[Command]
	void CmdThrowPackage() {
		transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
		transform.GetChild(0).GetComponent<Rigidbody>().AddForce(new Vector3(5000,5000,0));
		transform.DetachChildren();
	}

}
