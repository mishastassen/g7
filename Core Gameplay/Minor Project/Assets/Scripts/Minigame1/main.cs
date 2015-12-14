using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class main : NetworkBehaviour {
	
	// the 2 scripts
	public rotate left;
	public rotate right;

	//normal player prefab
	public GameObject prefab;

	// text and time and succes
	public Text winText;
	public Text loseText;
	public Text timeText;
	private float timeLeft;
	private double time;
	bool succes;
	bool finished;

	//Level to go back too
	//public string nextlevel;
	private string returnLevel;

	// Use this for initialization
	void Start () {
		timeLeft = 20.0f;
		time = timeLeft;
		winText.enabled = false;
		loseText.enabled = false;
		succes = false;
		finished = false;
		setTimeText ();
		returnLevel = Gamemanager.Instance.currentLevel;
	}
	
	// Update is called once per frame
	void Update () {
		if(left == null || right == null){
			return;
		}
		// if finished!
		if (left.finished && right.finished && !finished) {
			winText.enabled= true;
			succes = true;
			if(isServer){
				Gamemanager.Instance.onNextLevelLoad += triggerWin;	//Tigger chest completed event when previous level is loaded
			}
			finished = true;
		}

		if (!finished) {
			updateTime ();
		}
		setTimeText ();

		// if out of time
		if (timeLeft < 0 && !finished) {
			left.finished = true;
			right.finished = true;
			loseText.enabled = true;
			succes = false;
			finished = true;
		}

		if (isServer && finished && Input.GetButtonDown("Interact1_P1")) {
			NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
			Manager.playerPrefab = prefab;
			Debug.Log (returnLevel);
			Eventmanager.Instance.triggerLevelFinished(returnLevel);
		}
	}

	// timer
	void updateTime(){
		timeLeft -= Time.deltaTime;
		time = System.Math.Round (timeLeft, 1);
	}

	void setTimeText(){
		timeText.text = time.ToString ();
		if (time % 1 == 0) {
			timeText.text = time.ToString() + ".0";
		}
	}

	void triggerWin(){
		Time.timeScale = 1.0f;
		Eventmanager.Instance.triggerChestCompleted ();
		Debug.Log ("levelload");
	}
}


