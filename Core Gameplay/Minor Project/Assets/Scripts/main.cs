using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main : MonoBehaviour {
	
	// the 2 scripts
	public rotate left;
	public rotate right;

	// text and time and succes
	public Text winText;
	public Text loseText;
	public Text timeText;
	private float timeLeft;
	private double time;
	bool succes;

	// Use this for initialization
	void Start () {
		timeLeft = 20.0f;
		time = timeLeft;
		winText.enabled = false;
		loseText.enabled = false;
		succes = false;
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
			Time.timeScale = 0.0f;
		}

		updateTime ();
		setTimeText ();

		// if out of time
		if (timeLeft < 0) {
			loseText.enabled = true;
			succes = false;
			Time.timeScale = 0.0f;
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
}
