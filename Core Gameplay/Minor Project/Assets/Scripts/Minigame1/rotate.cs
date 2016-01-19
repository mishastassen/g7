using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class rotate : NetworkBehaviour {

	//PlayerId
	private int player;

	// set the GameObjects
	private GameObject cirkel;
	private main Main;
	
	// initialize the speed;
	public int speed;
	
	// boolean if the arrow is in the green part
	bool erin;
	
	// origin of the 2
	Vector3 pos_cirkel;
	Vector3 pos_pijl;
	
	// defines if the arrow goes CW or CCW
	[SyncVar]
	int way;
	
	// attributes for the score
	[SyncVar]
	private int count;
	private Text scoreText;
	public bool finished;

	//Input button
	private string inputButton = "Interact1_P1";
	private string redgreen;
	private string scoreTextName;

	//audio
	private AudioSource wrongSound;

	 void Start (){
		Debug.LogError (transform.position.x + " " + transform.position.y + " " + transform.position.z);
		if (transform.position.x == 0 && transform.position.y == 0 && transform.position.z == 0) {
			Destroy (this.gameObject);
		}
		if (this.transform.position.x < 0) {
			player = 1;
			redgreen = "redgreen_left";
			scoreTextName = "Score_left";
			GameObject.Find("main").GetComponent<main>().right = this;
		} else {
			player = 2;
			redgreen = "redgreen_right";
			scoreTextName = "Score_right";
			GameObject.Find("main").GetComponent<main>().left = this;
		}

		cirkel = transform.FindChild ("Circle").gameObject;
		scoreText = GameObject.Find (scoreTextName).GetComponent<Text>();
		pos_pijl = gameObject.transform.position;
		pos_cirkel = cirkel.transform.position;
		
		if (playerControllerId == 2) {
			inputButton = "Interact1_P2";
		}
		
		// Set initial variables and a random turn
		way = 1;
		erin = false;
		//RandomTurn ();
		count = 0;
		if (isLocalPlayer) {
			setScoreText ();
		}
		finished = false;
		speed = 150;

		wrongSound = GetComponent<AudioSource> ();
		Main = GameObject.Find ("main").GetComponent<main>();
	}

	// Update is called once per frame
	void Update () {
		if (Main.left == null || Main.right == null) {
			return;
		}
		// rotate the arrow every frame
		if (!finished) {
			gameObject.transform.RotateAround (pos_pijl, Vector3.forward, -speed * way * Time.deltaTime);
		}

		if (isLocalPlayer) {
			if (Input.GetKeyDown (KeyCode.Escape) && playerControllerId != 2) {
				GameObject uiManager = GameObject.Find ("LevelManager");
				uiManager.GetComponent<openEscMenu> ().triggerEscMenu ();
			}
		
			// if pressed correctly
			if (erin && Input.GetButtonDown(inputButton)) {
				way = way * -1;
				if(!isServer){
					CmdPressed(true);
				}
				RandomTurn ();
				count = count + 1;
				setScoreText ();
				GetComponent<PlayerAudioManager> ().succesSound.Play ();
			}
		
			// if not pressed correctly
			if (!erin && Input.GetButtonDown (inputButton)) {
				CmdPressed(false);
				count = 0;
				setScoreText ();
				GetComponent<PlayerAudioManager> ().failSound.Play ();
			}
		}

		if (count > 7) {
			way = 0;
			finished = true;
		}
	}
	
	// what happens when the arrow enters
	void OnTriggerEnter (Collider other){
		if (isLocalPlayer) {
			if (other.gameObject.name == redgreen) {
				erin = true;
			}
		}
	}
	
	// what happens when the arrow exits
	void OnTriggerExit (Collider other){
		if (isLocalPlayer) {
			if (other.gameObject.name == redgreen) {
				erin = false;
			}
		}
	}
	
	// random turn function
	void RandomTurn (){
		int move = Random.Range (0, 360);
		GameObject.Find (redgreen).transform.RotateAround (pos_cirkel, Vector3.forward, move );
		CmdRandomTurn (move);
	}

	[Command]
	void CmdRandomTurn(int move){
		GameObject.Find(redgreen).transform.RotateAround (pos_cirkel, Vector3.forward, move);
	}

	void setScoreText(){
		CmdSetScoreText (count.ToString ());
	}

	[Command]
	void CmdSetScoreText(string score){
		RpcSetScoreText (score);
	}

	[ClientRpc]
	void RpcSetScoreText(string score){
		Text scoretext = GameObject.Find (scoreTextName).GetComponent<Text>();
		scoretext.text = score;
	}

	[Command]
	void CmdPressed(bool erin){
		if (erin) {
			count = count + 1;
			way *= -1;
		} else {
			count = 0;
		}
		setScoreText ();
	}
}