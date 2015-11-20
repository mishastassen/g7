using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerContoller : NetworkBehaviour {

	[HideInInspector] public bool facingRight = true;
	private float speed;
	private float jump;
	public float runThreshold;

	[SyncVar]
	private bool hasPackage;

	private bool inRange;
	private Rigidbody rb;
	private Animator anim;

	private string horizontalAxis = "Horizontal_P1";
	private string jumpButton = "Jump_P1";
	private string interact1Button = "Interact1_P1";
	private string interact2Button = "Interact2_P1";
	private string throwButton = "Throw_P1";

	private float fastspeed;
	private float fastjump;
	private float slowspeed;
	private float slowjump;

	void Start () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator> ();
		Eventmanager.Instance.triggerPlayerAdded(this.gameObject);
		hasPackage = false;
		fastspeed = 10;
		fastjump = 21;
		slowspeed = 6;
		slowjump = 15;
		runThreshold = 0.5f;

		if (GetComponent<NetworkIdentity>().playerControllerId == 2){
			horizontalAxis = "Horizontal_P2";
			jumpButton = "Jump_P2";
			interact1Button = "Interact1_P2";
			interact2Button = "Interact2_P2";
			throwButton = "Throw_P2";
		}
	}
	
	void FixedUpdate () {
		if (!isLocalPlayer) { //Check if this is the player corresponding with the local client if not return
			return;
		}
		if (rb == null)
			return;

		// move player based on user input
		float moveHorizontal = Input.GetAxis (horizontalAxis);
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
		if (Input.GetButton(jumpButton) && (isGroundedToe() || isGroundedHeel())) {
			yVelocity = jump;
		}

		// move player
		Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);
		rb.velocity = movement;

		if (moveHorizontal < 0 && facingRight)
			Flip ();
		else if (moveHorizontal > 0 && !facingRight)
			Flip ();
		ManageAnimation (moveHorizontal);

		// drop the package
		if (Input.GetButton(interact2Button) && hasPackage) {
			transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
			transform.DetachChildren();
			hasPackage = false;
			CmdDropPackage();
		}

		// throw a package
		if (Input.GetButtonDown (throwButton) && hasPackage) {
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

	void Flip()	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.z *= -1;
		transform.localScale = theScale;
	}
	
	void ManageAnimation(float moveHorizontal) {
		bool isRunning = Mathf.Abs (speed * moveHorizontal) > runThreshold;
		anim.SetBool("isRunning", isRunning);
	}
	
    void OnDestroy()
    {
        Eventmanager.Instance.triggerPlayerRemoved(this.gameObject);
    }

	// pick up or catch a package
	void OnTriggerStay(Collider other) {
		if(Input.GetButton(interact1Button) && other.tag == "PickUp1" && !hasPackage)
		{	
			other.transform.parent.SetParent(rb.transform);
			other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
			other.transform.parent.localPosition = new Vector3(0,3,2);
			CmdPickupPackage("PickUp1");
			hasPackage = true;
		}
	}

	[Command]
	void CmdPickupPackage(string tag){
		GameObject other = GameObject.FindWithTag(tag);
		other.transform.parent.SetParent(rb.transform);
		other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
		other.transform.parent.localPosition = new Vector3(0,3,2);
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
