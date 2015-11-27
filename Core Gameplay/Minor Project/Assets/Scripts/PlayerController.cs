using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : NetworkBehaviour {
	
	private float speed;
	private float jump;
	public float runThreshold;
	
	[SyncVar]
<<<<<<< HEAD:Core Gameplay/Minor Project/Assets/Scripts/PlayerController.cs
	public bool hasPackage, walking;
	public Transform carriedPackage;
	
	public float facingRight;
=======
	private bool hasPackage, walking;
	private Transform carriedPackage;

	[SyncVar(hook="OnFacingChange")]
	private float facingRight;
	[SyncVar(hook="OnAnimationChange")]
	private bool isRunning;
>>>>>>> 96d444e9f6ebf4a60ff074b879d364c1ee22f133:Core Gameplay/Minor Project/Assets/Scripts/PlayerContoller.cs
	private bool PlayWalkingSoundrunning;
	private bool doJump = false;
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

	//The list of triggers currently inside the player
	public List<Collider> TriggerList= new List<Collider>();
	
	
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

	void Update(){
		if (isLocalPlayer) {
			//Check interact1 button
			if (Input.GetButtonDown (interact1Button)) {
				doInteract1 ();
			}

			if(Input.GetButtonDown (throwButton)){
				doThrowButton();
			}
			// jump based on user input
			if (Input.GetButton (jumpButton) && (isGroundedToe () || isGroundedHeel ())) {
				doJump = true;
			}
		}
	}

	void FixedUpdate () {
		if (isLocalPlayer) { //Check if this is the player corresponding with the local client
			if (rb == null)
				return;
			
			// move player based on user input
			float moveHorizontal = Input.GetAxis (horizontalAxis);
			float yVelocity = rb.velocity.y;

			if(doJump){
				yVelocity = jump;
				doJump = false;
			}
			
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
			
			// move player
			Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);
			rb.velocity = movement;
<<<<<<< HEAD:Core Gameplay/Minor Project/Assets/Scripts/PlayerController.cs

			//Play walking sound if player is on the ground
			if (walking == true && (isGroundedToe () || isGroundedHeel ()) && !PlayWalkingSoundrunning) {
				StartCoroutine (PlayWalkingSound ());
			}
=======
			
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
			CmdCheckFacing (moveHorizontal);
			CmdCheckAnimation (moveHorizontal);
>>>>>>> 96d444e9f6ebf4a60ff074b879d364c1ee22f133:Core Gameplay/Minor Project/Assets/Scripts/PlayerContoller.cs
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


	[Command]
	void CmdCheckFacing(float moveHorizontal) {
		if (moveHorizontal < 0)
			facingRight = -1;
		if (moveHorizontal > 0)
			facingRight = 1;
	}

	void OnFacingChange(float facingRight) {
		Vector3 theScale = transform.localScale;
		theScale.z = facingRight;
		transform.localScale = theScale;
	}

	[Command]
	void CmdCheckAnimation(float moveHorizontal) {
		if (moveHorizontal == 0)
			isRunning = false;
		else
			isRunning = true;
	}

	void OnAnimationChange(bool isRunning) {
		anim.SetBool ("isRunning", isRunning);
	}

	//Trigger player removed event
	void OnDestroy()
	{
		Eventmanager.Instance.triggerPlayerRemoved(this.gameObject);
	}

	//Add triggers to trigger list
	void OnTriggerEnter(Collider other)
	{
		//if the object is not already in the list
		if(!TriggerList.Contains(other))
		{
			//add the object to the list
			TriggerList.Add(other);
		}
	}

	//Remove triggers from trigger list
	void OnTriggerExit(Collider other)
	{
		//if the object is in the list
		if(TriggerList.Contains(other))
		{
			//remove it from the list
			TriggerList.Remove(other);
		}
	}

	void doThrowButton(){
		if (hasPackage) {
			CmdThrowPackage ();
		}
	}

	void doInteract1(){
		if (hasPackage) {
			CmdDropPackage ();
		}
		else{
			if (TriggerList.Exists (x => x.tag == "PickUp1")) {
				CmdPickupPackage ("PickUp1");
			}

			if (TriggerList.Exists (x => x.tag == "Switch")) {
				CmdTriggerSwitch ();
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
		Eventmanager.Instance.packagePickup (this.gameObject,tag);
	}
	
	[Command]
	void CmdDropPackage(){
		Eventmanager.Instance.packageDrop (this.gameObject);
	}
	
	[Command]
	void CmdThrowPackage() {
		Eventmanager.Instance.packageThrow (this.gameObject);
	}

	[Command]
	void CmdTriggerSwitch(){
		Eventmanager.Instance.triggerSwitchPulled();
	}

}