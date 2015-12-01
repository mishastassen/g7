using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class rotate : NetworkBehaviour {

	//PlayerId
	private int player;

	// set the GameObjects
	private GameObject cirkel;
	
	// initialize the speed;
	public int speed;
	
	// boolean if the arrow is in the green part
	bool erin;
	
	// origin of the 2
	Vector3 pos_cirkel;
	Vector3 pos_pijl;
	
	// defines if the arrow goes CW or CCW
	int way;
	
	// attributes for the score
	private int count;
	private Text scoreText;
	public bool finished;

	//Input button
	private string inputButton = "Interact1_P1";
	private string redgreen;
	private string scoreTextName;

	 void Start (){
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
		setScoreText ();
		finished = false;
		speed = 150;
	}
	
	// Update is called once per frame
	void Update () {
		// rotate the arrow every frame
		gameObject.transform.RotateAround (pos_pijl, Vector3.forward, -speed * way * Time.deltaTime);
		if (isLocalPlayer) {
		
			// if pressed correctly
			if (erin && Input.GetButtonDown(inputButton)) {
				way = way * -1;
				if(!isServer){
					CmdChangeWay();
				}
				RandomTurn ();
				count = count + 1;
				setScoreText ();
			}
		
			// if not pressed correctly
			if (!erin && Input.GetButtonDown (inputButton)) {
				count = 0;
				setScoreText ();
			}
		
			if (count > 7) {
				way = 0;
				finished = true;
			}
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
		scoreText.text = count.ToString ();
		CmdSetScoreText (count.ToString ());
	}

	[Command]
	void CmdSetScoreText(string score){
		Text scoretext = GameObject.Find (scoreTextName).GetComponent<Text>();
		scoretext.text = score;
	}

	[Command]
	void CmdChangeWay(){
		way *= -1;
	}
	
}