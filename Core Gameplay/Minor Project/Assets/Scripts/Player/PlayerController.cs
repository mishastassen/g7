using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityAnalyticsHeatmap;
//using System.Linq;

public class PlayerController : NetworkBehaviour  {

	private float speed;
	private float jump, lowjump;
	public float runThreshold;
	
	[SyncVar]
	public bool hasPackage, hasMagicPackage;
	public bool walking;
	public Transform carriedPackage;

	[SyncVar(hook="OnAnimationChange")]
	private int animationID=1; // 1: facingRight == 1 and idle

	//[SyncVar(hook="OnFacingChange")]
	public float facingRight;
	Quaternion targetRotation;

	//[SyncVar(hook="OnAnimationChange")]
	private bool isRunning;
	private float startTimeJump;
	//[SyncVar(hook="OnJumpingChange")]
	private bool isJumping;
	private float startTimeDraw;
	//[SyncVar(hook="OnDrawingChange")]
	private bool isDrawing;
	private bool isGrounded;
	private bool PlayWalkingSoundrunning;
	private bool doJump = false;
	private bool doJumpCancel = false;
	private Rigidbody rb;
	private Animator anim;
	
	private string horizontalAxis = "Horizontal_P1";
	private string jumpButton = "Jump_P1";
	private string interact1Button = "Interact1_P1";
	private string interact2Button = "Interact2_P1";
	private string throwButton = "Throw_P1";
	
	private float fastspeed;
	private float fastjump; // velocity for highest jump without package
	private float fastjumplow; // velocity for lowest jump without package
	private float slowspeed;
	private float slowjump; // velocity for highest jump with package
	private float slowjumplow; // velocity for lowest jump with package
	public float turnSpeed;
	
	public Text debugText;
	private int playerID;

	//The list of triggers the player is currently in
	public List<Collider> TriggerList= new List<Collider>();
	
	
	void Start() {
		Time.timeScale = 1.0f;
		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator> ();
		if (isLocalPlayer) {
			Eventmanager.Instance.triggerPlayerAdded (this.gameObject);
		}
		hasPackage = false;
		hasMagicPackage = false;
		carriedPackage = null;
		fastspeed = 12;
		fastjump = 22;
		fastjumplow = 11;
		slowspeed = 8;
		slowjump = 18;
		slowjumplow = 9;
		turnSpeed = 12;
		runThreshold = 0.5f;
		facingRight = 1;
		isDrawing = false;

		GameObject debugObject = GameObject.Find ("DebugText");
		if(debugObject!=null)
			debugText = debugObject.GetComponent<Text>();

		playerID = 1;
		if (GetComponent<NetworkIdentity>().playerControllerId == 2){
			playerID = 2;
			horizontalAxis = "Horizontal_P2";
			jumpButton = "Jump_P2";
			interact1Button = "Interact1_P2";
			interact2Button = "Interact2_P2";
			throwButton = "Throw_P2";
		}
		if (WebManager.Instance != null) {
			if (isLocalPlayer && WebManager.Instance.otherPlayer != null) {
				changeColour (WebManager.Instance.currentUser.playerColor);
			} else if (!isLocalPlayer && WebManager.Instance.otherPlayer != null) {
				changeColour (WebManager.Instance.otherPlayer.playerColor);
			} else if (playerID == 1) {
				changeColour (WebManager.Instance.currentUser.playerColor);
			}
		}
	}

	void Update(){
		if (isLocalPlayer) {
			CheckGrounded();

			//Check interact1 button
			if (Input.GetButtonDown (interact1Button)) {
				doInteract1 ();
			}

			if(Input.GetButtonDown (throwButton)){
				doThrowButton();
			}
			// jump based on user input
			if (Input.GetButtonDown (jumpButton) && isGrounded) {
				doJump = true;
			}
			if (Input.GetButtonUp (jumpButton)) {
				doJumpCancel = true;
			}
			if(Input.GetButtonDown (interact2Button)) {
				doInteract2();
			}

			if (Input.GetKeyDown (KeyCode.Escape) && playerID == 1) {
				GameObject uiManager = GameObject.Find ("LevelManager");
				uiManager.GetComponent<openEscMenu> ().triggerEscMenu ();
			}
		}

		// Smoothly rotate towards the target point
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

	}

	void FixedUpdate () {
		if (isLocalPlayer) { //Check if this is the player corresponding with the local client
			CheckGrounded();

			// move player based on user input
			float moveHorizontal = Input.GetAxis (horizontalAxis);
			float yVelocity = rb.velocity.y;

			if(doJump){
				yVelocity = jump;
				doJump = false;
				//CmdCheckJumping (true);
				isJumping = true; //???
				startTimeJump = Time.time;
			}
			if(doJumpCancel){
				if(yVelocity>lowjump)
					yVelocity = lowjump;
				doJumpCancel = false;
			}

			//*
			// not the way it should be done
			if(isDrawing && startTimeDraw+1<Time.time) {
				//CmdCheckDrawing(false);
				isDrawing = false;
			}
			//*/
			//isDrawing = false;

			//Check if player is walking
			if (Mathf.Abs (moveHorizontal) > 0.1) {
				walking = true;
			} else {
				walking = false;
			}
			
			// set speed and jumppower
			if (hasPackage) {
				speed = slowspeed;
				jump = slowjump;
				lowjump = slowjumplow;
			} else {
				speed = fastspeed;
				jump = fastjump;
				lowjump = fastjumplow;
			}

			// reverse walking
			if (hasMagicPackage) {
				moveHorizontal *= -1;
			}
			
			// move player
			Vector3 movement = new Vector3 (speed * moveHorizontal, yVelocity, 0.0f);
			rb.velocity = movement;

			//Play walking sound if player is on the ground
			if (walking == true && isGrounded && !PlayWalkingSoundrunning) {
				StartCoroutine (PlayWalkingSound ());
			}

			int nextAnimationID = CalcAnimationID ();
			if (nextAnimationID != animationID)
				CmdChangeAnimation (nextAnimationID);
			CheckAnimation (moveHorizontal);
			//Debug.Log(isJumping+" "+isInStartJump()+" "+isGrounded);
			if(debugText!=null)
			{
				//debugText.text = "isJumping: "+isJumping+", isInStartJump(): "+isInStartJump()+", isGrounded: "+isGrounded;
				debugText.text = isJumping+" "+isInStartJump()+" "+isGrounded;
			}
			if (isJumping && !isInStartJump () && isGrounded) {
				//CmdCheckJumping (false);
				isJumping = false;//???
			}
		}
	}

	void CheckAnimation(float moveHorizontal) {
		if (moveHorizontal < 0) {
			facingRight = -1;
			isRunning = true;
		} else if (moveHorizontal == 0) {
			isRunning = false;
		} else if (moveHorizontal > 0) {
			facingRight = 1;
			isRunning = true;
		}
	}


	bool isInStartJump() {
		//Debug.Log ("isInStartJump: "+(Time.time - startTimeJump < 0.3f));
		return Time.time - startTimeJump < 0.3f;
	}


	int CalcAnimationID() {
		int res = 0;
		if (facingRight > 0)
			res += 1;
		if (isRunning)
			res += 2;
		if (isJumping)
			res += 4;
		if (isDrawing)
			res += 8;
		return res;
	}

	[Command]
	private void CmdChangeAnimation(int nextAnimationID) {
		animationID = nextAnimationID;
	}

	void OnAnimationChange(int nextID) {
		facingRight = nextID % 2 == 1 ? 1 : -1;
		isRunning = (nextID / 2) % 2 == 1;
		isJumping =  (nextID / 4) % 2 == 1;
		isDrawing =  (nextID / 8) % 2 == 1;

		targetRotation = Quaternion.LookRotation(facingRight*Vector3.forward);
		anim.SetBool ("isRunning", isRunning);
		anim.SetBool ("isJumping", isJumping);
		anim.SetBool ("isDrawing", isDrawing);

		animationID = nextID;
	}

	// invoke at start of update and fixedupdate to set bool isGrounded
	void CheckGrounded() {
		//isGrounded = true;
		isGrounded = isGroundedHeel () || isGroundedToe ();
		//Debug.Log ("isgrounded: "+isGrounded);
	}

	// checks whether the front of the player is on a platform
	bool isGroundedToe() {
		Vector3 toePosition = new Vector3(rb.transform.position.x + 0.5f, rb.transform.position.y+0.1f, rb.transform.position.z);
		Debug.DrawRay (toePosition, -Vector3.up, Color.red);
		return Physics.Raycast (toePosition, -Vector3.up, 0.2f);
	}
	
	// checks whether the back of the player is on a platform
	bool isGroundedHeel() {
		Vector3 heelPosition = new Vector3(rb.transform.position.x - 0.5f, rb.transform.position.y+0.1f, rb.transform.position.z);
		Debug.DrawRay (heelPosition, -Vector3.up, Color.blue);
		return Physics.Raycast (heelPosition, -Vector3.up, 0.2f);
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
		// If player gets hit by an enemy
		if (other.tag == "Naginata" && isLocalPlayer) {
			CmdDeath ();
		}
		if (other.tag == "DeathZone" && isLocalPlayer) {
			CmdDeath ();
		} 
		if (other.tag == "Checkpoint" && isServer) {
			CmdCheckpointReached(other.GetComponent<CheckpointController>().checkpointNum);
		}
		else { //Add collider to list of triggers player is standing in
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
		// If an object in triggerlist gets destroyed, OnTriggerExit isn't called, but object should be removed
		TriggerList.RemoveAll(x => x == null);

		if (hasPackage) {
			CmdDropPackage ();
		}
		else{
			if (TriggerList.Exists (x => x.tag == "PickUp1")) {
				CmdPickupPackage ("PickUp1");
			}

			if (TriggerList.Exists (x => x.tag == "PickUpMagic")) {
				CmdPickupPackage ("PickUpMagic");
			}

			foreach( Collider c in TriggerList) {
				//Debug.Log (c.tag);
				if(c.tag == "Switch") {
					int switchID = ExtractIDFromName(c.name);
					Debug.Log ("switch collider.name: "+c.name+", ID: "+switchID);
					CmdTriggerSwitch(switchID);
				}
			}

			if(TriggerList.Exists (x => x.tag == "Chest")){
				CmdTriggerChest();
			}

			if (TriggerList.Exists (x => x.tag == "startMinigame3")) {
				CmdTriggerMinigame3 ();
			}
		}
	}

	void doInteract2() {
		// Rule: You can't hit while carrying a package
		if (hasPackage)
			return;
		if (!isDrawing) {
			isDrawing = true;
			startTimeDraw = Time.time;
		}
	}
		
	void changeColour(Color32 playerColor){
		Texture2D texture = this.transform.GetChild (0).GetChild (1).gameObject.GetComponent<SkinnedMeshRenderer> ().material.mainTexture as Texture2D;
		Texture2D newTexture = new Texture2D (texture.width, texture.height, TextureFormat.ARGB32, true);
		Color32[] colors = texture.GetPixels32 (0);
		for(int j = 0; j < colors.Length; j++){
			if ((196 > colors[j].r && colors[j].r > 186) && (6 > colors [j].g) && (169 > colors [j].b && colors [j].b > 159)) {
				colors [j] = playerColor;
			}
		}
		newTexture.SetPixels32(colors,0);
		newTexture.Apply(true);
		this.transform.GetChild (0).GetChild (1).gameObject.GetComponent<SkinnedMeshRenderer> ().material.SetTexture ("_MainTex",newTexture);
	}

	//Play walking sound
	IEnumerator PlayWalkingSound(){
		PlayWalkingSoundrunning = true;
		GetComponent<PlayerAudioManager> ().audioFootstepWood1.Play ();
		float delay = (12.0f-rb.velocity.magnitude)*0.1f;
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
		/*
		Analytics.CustomEvent ("Package Picked Up", new Dictionary<string , object> {
			{"Levelname", Gamevariables.currentLevel}
		});
		*/
	}

	[Command]
	void CmdDropPackage(){
		Eventmanager.Instance.packageDrop (this.gameObject);
		/*
		Analytics.CustomEvent ("Package Dropped", new Dictionary<string , object> {
			{"Levelname", Gamevariables.currentLevel}
		});
		*/
	}
	
	[Command]
	void CmdThrowPackage() {
		Eventmanager.Instance.packageThrow (this.gameObject);
		/*
		Analytics.CustomEvent ("Package Thrown", new Dictionary<string , object> {
			{"Levelname", Gamevariables.currentLevel}
		});
		*/
	}

	[Command]
	void CmdTriggerSwitch(int id){
		Eventmanager.Instance.triggerSwitchPulled(id);
		/*
		Analytics.CustomEvent ("Switch Triggered", new Dictionary<string , object> {
			{"Levelname", Gamevariables.currentLevel}
		});
		*/
	}

	[Command]
	void CmdTriggerChest(){
		Eventmanager.Instance.triggerChestActivated ();
		/*
		Analytics.CustomEvent ("Chest Minigame Started", new Dictionary<string , object> {
			{"Levelname", Gamevariables.currentLevel},
			{ "Time", Gamevariables.timer }
		});
		*/
	}

	[Command]
	void CmdTriggerMinigame3(){
		Eventmanager.Instance.triggerMinigame3Activated ();
	}

	[Command]
	void CmdDeath(){
		Eventmanager.Instance.triggerPlayerDeath (this.gameObject);
		/*
		Analytics.CustomEvent ("Player Deaths", new Dictionary<string , object> {
			{"Levelname", Gamevariables.currentLevel}
		});
		HeatmapEvent.Send("Player Death Heatmap", this.transform.position, new Dictionary<string, object> {
			{"Levelname", Gamevariables.currentLevel}
		});
		*/
	}

	[Command]
	void CmdCheckpointReached(int checkpointNum){
		Eventmanager.Instance.triggerCheckpointReached (checkpointNum);
		/*
		Analytics.CustomEvent ("Checkpoint Reached", new Dictionary<string , object> {
			{"Levelname", Gamevariables.currentLevel},
			{"Checkpoint number", checkpointNum},
			{"Time", Gamevariables.timer}
		});
		*/
	}

	[Command]
	public void CmdResetToLastCheckpoint(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			if (!player.Equals (gameObject)) {
				Eventmanager.Instance.triggerPlayerDeath (player);
			}
		}
		Eventmanager.Instance.triggerPackageDestroyed ();
		Eventmanager.Instance.triggerPlayerDeath (gameObject);
	}

}