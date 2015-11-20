using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerContoller : NetworkBehaviour {

	private float speed;
	private float jump;

	[SyncVar]
	private bool hasPackage;

	private bool inRange;
	private Rigidbody rb;

	private string horizontalAxis = "Horizontal_P1";
	private string jumpButton = "Jump_P1";
	private string interact1Button = "Interact1_P1";
	private string interact2Button = "Interact2_P1";

	public float fastspeed;
	public float fastjump;

	public float slowspeed;
	public float slowjump;

	void Start () {
		rb = GetComponent<Rigidbody>();
		Eventmanager.Instance.triggerPlayerAdded(this.gameObject);
		hasPackage = false;
		if (GetComponent<NetworkIdentity>().playerControllerId == 2){
			horizontalAxis = "Horizontal_P2";
			jumpButton = "Jump_P2";
			interact1Button = "Interact1_P2";
			interact2Button = "Interact2_P2";
		}
	}
	
	void FixedUpdate () {
		if (!isLocalPlayer) { //Check if this is the player corresponding with the local client if not return
			return;
		}
		if (rb == null)
			return;

		float moveHorizontal = Input.GetAxis (horizontalAxis);
		float yVelocity = rb.velocity.y;

		if (hasPackage) {
			speed = slowspeed;
			jump = slowjump;
		} else {
			speed = fastspeed;
			jump = fastjump;
		}

		if (Input.GetButton(jumpButton) && (isGroundedToe() || isGroundedHeel())) {
			yVelocity = jump;
		}

		Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);

		rb.velocity = movement;

		if (Input.GetButton(interact2Button) && hasPackage) {
			transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
			transform.DetachChildren();
			hasPackage = false;
			CmdDropPackage();
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
		if(Input.GetButton(interact1Button) && other.tag == "PickUp1" && !hasPackage)
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


}
