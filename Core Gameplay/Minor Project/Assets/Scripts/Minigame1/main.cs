using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
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
	private int amountofTries;

	//Level to go back too
	//public string nextlevel;
	private string returnLevel;
	private bool instructionsFinished;

	//instruction variables go below
	public Canvas instructions;
	public Text disappearTimeText;
	private int disappeartimeleft;
	private float disappear = 15f;

	// Use this for initialization
	void Start () {
		instructionsFinished = false;
		returnLevel = Gamevariables.returnLevel;
		instructions.enabled = false;
		if (returnLevel == "Level1C") {
			StartCoroutine (instructionsTextFade ());
		} else {
			instructionsFinished = true;
		}
		timeLeft = 20.0f;
		time = timeLeft;
		winText.enabled = false;
		loseText.enabled = false;
		succes = false;
		finished = false;
		setTimeText ();
		amountofTries = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (instructionsFinished) {
			if (left == null || right == null) {
				return;
			}
			// if finished!
			if (left.finished && right.finished && !finished) {
				winText.enabled = true;
				succes = true;
				if (isServer) {
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

			if (isServer && finished && Input.GetButtonDown ("Interact1_P1")) {
				if (succes) {
					NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager> ();
					Manager.playerPrefab = prefab;
					Analytics.CustomEvent ("Minigame Finished", new Dictionary<string, object> {
						{ "Time Left", timeLeft },
						{ "Mingame Tries", amountofTries }
					});
					Eventmanager.Instance.triggerLevelSwitch (returnLevel);
				} else {
					amountofTries += 1;
					Eventmanager.Instance.triggerLevelSwitch ("Minigame1");
				}
			}
		} else {
			setDisappearText ();
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

	void setDisappearText (){
		disappeartimeleft = (int)(disappear -= Time.deltaTime);
		disappearTimeText.text = ("This screen will automatically disappear in " + disappeartimeleft.ToString() +" seconds");
	}

	void triggerWin(){
		Time.timeScale = 1.0f;
		Eventmanager.Instance.triggerChestCompleted ();
		Debug.Log ("levelload");
	}

	IEnumerator instructionsTextFade(){
		instructions.enabled = true;
		yield return new WaitForSeconds (15f);
		instructions.enabled = false;
		instructionsFinished = true;
	}
}


