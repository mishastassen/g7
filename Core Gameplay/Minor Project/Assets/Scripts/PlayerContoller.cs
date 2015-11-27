using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerContoller : NetworkBehaviour {
	
	private float speed;
	private float jump;
	public float runThreshold;
	
	[SyncVar]
	private bool hasPackage, walking;
	private Transform carriedPackage;
	
	private float facingRight;
	private bool PlayWalkingSoundrunning;
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
	
	private int footstep = 1;
	
	
	void Start () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator> ();
		Eventmanager.Instance.triggerPlayerAdded(this.gameObject);
		hasPackage = false;
		carriedPackage = null;
		fastspeed = 10;
		fastjump = 25;
		slowspeed = 6;
		slowjump = 20;
		runThreshold = 0.5f;
		facingRight = 1;
		
		if (GetComponent<NetworkIdentity>().playerControllerId == 2){
			horizontalAxis = "Horizontal_P2";
			jumpButton = "Jump_P2";
			interact1Button = "Interact1_P2";
			interact2Button = "Interact2_P2";
			throwButton = "Throw_P2";
		}
	}
	
	void FixedUpdate () {
		if (isLocalPlayer) { //Check if this is the player corresponding with the local client if not return
			if (rb == null)
				return;
			
			// move player based on user input
			float moveHorizontal = Input.GetAxis (horizontalAxis);
			float yVelocity = rb.velocity.y;
			
			//Sync if players are walking
			if (Mathf.Abs (moveHorizontal) > 0.1) {
				walking = true;
			} else {
				walking = false;
			}
			
			// set speed and jumppower
			if (hasPackage) {
				speed = slowspeed;
				jump = slowjump;
			} else {
				speed = fastspeed;
				jump = fastjump;
			}
			
			// jump based on user input
			if (Input.GetButton (jumpButton) && (isGroundedToe () || isGroundedHeel ())) {
				yVelocity = jump;
			}
			
			// move player
			Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);
			rb.velocity = movement;
			
			/*
			if (movement.x < 0 && facingRight)
				Flip ();
			else if (moveHorizontal > 0 && !facingRight)
				Flip ();
			*/
			
			//Play walking sound if player is ont the ground
			if (walking == true && (isGroundedToe () || isGroundedHeel ()) && !PlayWalkingSoundrunning) {
				StartCoroutine (PlayWalkingSound ());
			}
			
			// drop the package
			if (Input.GetButtonDown (interact1Button) && hasPackage) {
				Debug.Log ("player "+playerControllerId+" drops package.");
				carriedPackage.GetComponent<Rigidbody> ().isKinematic = false;
				carriedPackage.parent = null;
				
				//transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
				//transform.DetachChildren();
				hasPackage = false;
				//carriedPackage = null;
				CmdDropPackage ();
			}
			
			// throw a package
			if (Input.GetButtonDown (throwButton) && hasPackage) {
				carriedPackage.GetComponent<Rigidbody> ().isKinematic = false;
				carriedPackage.GetComponent<Rigidbody> ().AddForce (new Vector3 (facingRight * 500, 500, 0));
				carriedPackage.parent = null;
				//transform.GetChild (0).GetComponent<Rigidbody> ().isKinematic = false;
				//transform.GetChild (0).GetComponent<Rigidbody> ().AddForce (new Vector3 (5000, 5000, 0));
				//transform.DetachChildren ();			
				hasPackage = false;
				CmdThrowPackage ();
			}
		}
		ManageAnimation ();
		Flip ();
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
		Vector3 theScale = transform.localScale;
		if ((rb.velocity.x < 0 && theScale.z > 0) || (rb.velocity.x > 0 && theScale.z < 0)){
			theScale.z *= -1;
			transform.localScale = theScale;
			facingRight *= -1;
		}
	}
	
	void ManageAnimation() {
		if (Mathf.Abs (rb.velocity.x) > runThreshold) {
			anim.SetBool ("isRunning", true);
		} else {
			anim.SetBool ("isRunning", false);
		}
	}
	
	//Trigger player removed event
	void OnDestroy()
	{
		//Eventmanager.Instance.triggerPlayerRemoved(this.gameObject);
	}
	
	// pick up or catch a package
	void OnTriggerStay(Collider other) {
		if(Input.GetButtonDown(interact1Button) && other.tag == "PickUp1" && !hasPackage && isLocalPlayer)
		{	
			Debug.Log ("player "+playerControllerId+" picks up package.");
			other.transform.parent.SetParent(rb.transform);
			other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
			other.transform.parent.localPosition = new Vector3(0,3,2);
			CmdPickupPackage("PickUp1");
			hasPackage = true;
			carriedPackage = other.transform.parent;
		}

		if (Input.GetButtonDown (interact1Button) && other.tag == "Switch" && isLocalPlayer) {
			if(!isServer){
				Eventmanager.Instance.triggerSwitchPulled();
				CmdTriggerSwitch ();
			}
			else{
				RpcTriggerSwitch();
			}
		}

	}
	
	//Play walking sound
	IEnumerator PlayWalkingSound(){
		PlayWalkingSoundrunning = true;
		GetComponent<PlayerAudioManager> ().audioFootstepWood1.Play ();
		float delay = (10.0f-rb.velocity.magnitude)*0.1f;
		if (delay > 0.15){
			delay = 0.15f;
		}
		yield return new WaitForSeconds (0.323f + delay);
		if (walking == true && (isGroundedToe () || isGroundedHeel ())) {
			GetComponent<PlayerAudioManager> ().audioFootstepWood2.Play ();
			delay = (10.0f-rb.velocity.magnitude)*0.1f;
			if (delay > 0.15){
				delay = 0.15f;
			}
			yield return new WaitForSeconds (0.323f + delay);
		}
		PlayWalkingSoundrunning = false;
	}
	
	[Command]
	void CmdPickupPackage(string tag){
		Debug.Log ("[Command] player "+playerControllerId+" picks up package.");
		GameObject other = GameObject.FindWithTag(tag);
		other.transform.parent.SetParent(rb.transform);
		other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
		other.transform.parent.localPosition = new Vector3(0,3,2);
		carriedPackage = other.transform.parent;
	}
	
	[Command]
	void CmdDropPackage(){
		Debug.Log ("[Command] player "+playerControllerId+" drops package.");
		carriedPackage.GetComponent<Rigidbody>().isKinematic = false;
		carriedPackage = null;
	}
	
	[Command]
	void CmdThrowPackage() {
		carriedPackage.GetComponent<Rigidbody> ().isKinematic = false;
		carriedPackage.GetComponent<Rigidbody> ().AddForce (new Vector3 (facingRight * 500, 500, 0));
		carriedPackage.parent = null;
	}

	[Command]
	void CmdTriggerSwitch(){
		Eventmanager.Instance.triggerSwitchPulled();
	}

	[ClientRpc]
	void RpcTriggerSwitch(){
		Eventmanager.Instance.triggerSwitchPulled();
	}
	
}