using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
//using System.Linq;

public class PlayerController : NetworkBehaviour {
	
	private float speed;
	private float jump;
	public float runThreshold;
	
	[SyncVar]
	public bool hasPackage, walking;
	public Transform carriedPackage;

	[SyncVar(hook="OnFacingChange")]
	public float facingRight;

	[SyncVar(hook="OnAnimationChange")]
	private bool isRunning;
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

	//The list of triggers the player is currently in
	public List<Collider> TriggerList= new List<Collider>();
	
	
	void Start() {
		Time.timeScale = 1.0f;
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator> ();
		Eventmanager.Instance.triggerPlayerAdded(this.gameObject);
		hasPackage = false;
		carriedPackage = null;
		fastspeed = 12;
		fastjump = 22;
		slowspeed = 8;
		slowjump = 18;
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

	void OnEnable(){
		Eventmanager.Instance.triggerPlayerAdded(this.gameObject);
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

			//Play walking sound if player is ont the ground
			if (walking == true && (isGroundedToe () || isGroundedHeel ()) && !PlayWalkingSoundrunning) {
				StartCoroutine (PlayWalkingSound ());
			}

			CmdCheckFacing (moveHorizontal);
			CmdCheckAnimation (moveHorizontal);
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
	void OnDisable()
	{
		if (Eventmanager.Instance != null) {
			Eventmanager.Instance.triggerPlayerRemoved (this.gameObject);
		}
	}

	//Add triggers to trigger list
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "DeathZone" && isServer) {
			CmdDeath ();
		} else { //Add collider to list of triggers player is standing in
			//if the object is not already in the list
			if (!TriggerList.Contains (other)) {
				//add the object to the list
				TriggerList.Add (other);
			}
		}
	}

	//Remove triggers from trigger list
	void OnTriggerExit(Collider other)
	{
		//if the object is in the list of triggers, remove it
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

	int ExtractIDFromName(String name) {
		int from = name.IndexOf ('(')+1;
		int to = name.IndexOf (')');
		int res=-1;
		if (from<to && int.TryParse(name.Substring (from,to-from), out res))
			return res;
		return res;
	}


	void doInteract1(){
		if (hasPackage) {
			CmdDropPackage ();
		}
		else{
			if (TriggerList.Exists (x => x.tag == "PickUp1")) {
				CmdPickupPackage ("PickUp1");
			}
			/*
			if (TriggerList.Exists (x => x.tag == "PickUpMagic")) {
				CmdPickupMagicPackage ("PickUpMagic");
			}
			*/
			foreach( Collider c in TriggerList) {
				if(c.tag == "Switch") {
					int switchID = ExtractIDFromName(c.name);
					Debug.Log ("switch collider.name: "+c.name+", ID: "+switchID);
					CmdTriggerSwitch(switchID);
				}
			}

			if(TriggerList.Exists (x => x.tag == "Chest")){
				CmdTriggerChest();
			}
		}
	}

	//Play walking sound
	IEnumerator PlayWalkingSound(){
		PlayWalkingSoundrunning = true;
		GetComponent<PlayerAudioManager> ().audioFootstepWood1.Play ();
		float delay = (11.0f-rb.velocity.magnitude)*0.1f;
		if (delay > 0.15){
			delay = 0.15f;
		}
		yield return new WaitForSeconds (0.323f + delay);
		if (walking == true && (isGroundedToe () || isGroundedHeel ())) {
			GetComponent<PlayerAudioManager> ().audioFootstepWood2.Play ();
			delay = (11.0f-rb.velocity.magnitude)*0.1f;
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
	/*
	[Command]
	void CmdPickupMagicPackage(string tag){
		Eventmanager.Instance.packagePickupMagica (this.gameObject,tag);
	}
	*/
	[Command]
	void CmdDropPackage(){
		Eventmanager.Instance.packageDrop (this.gameObject);
	}
	
	[Command]
	void CmdThrowPackage() {
		Eventmanager.Instance.packageThrow (this.gameObject);
	}

	[Command]
	void CmdTriggerSwitch(int id){
		Eventmanager.Instance.triggerSwitchPulled(id);
	}

	[Command]
	void CmdTriggerChest(){
		Eventmanager.Instance.triggerChestActivated ();
	}

	[Command]
	void CmdDeath(){
		Eventmanager.Instance.triggerPlayerDeath (this.gameObject);
	}

}