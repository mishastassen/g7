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
	public string nextlevel;
	
	// Use this for initialization
	void Start () {
		timeLeft = 20.0f;
		time = timeLeft;
		winText.enabled = false;
		loseText.enabled = false;
		succes = false;
		finished = false;
		setTimeText ();
	}
	
	// Update is called once per frame
	void Update () {
		if(left == null || right == null){
			return;
		}
		// if finished!
		if (left.finished && right.finished) {
			winText.enabled= true;
			succes = true;
			if(isServer && !finished){
				Gamemanager.Instance.onNextLevelLoad += triggerWin;	//Tigger chest completed event when previous level is loaded
			}
			finished = true;
			Time.timeScale = 0.0f;
		}

		updateTime ();
		setTimeText ();

		// if out of time
		if (timeLeft < 0) {
			loseText.enabled = true;
			succes = false;
			Time.timeScale = 0.0f;
			finished = true;
		}

		if (isServer && finished && Input.GetButtonDown("Interact1_P1")) {
			NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
			Manager.playerPrefab = prefab;
			Eventmanager.Instance.triggerLevelFinished(nextlevel);
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
		//Eventmanager.Instance.triggerChestCompleted ();
		Time.timeScale = 1.0f;
		Debug.Log ("levelload");
	}
}
