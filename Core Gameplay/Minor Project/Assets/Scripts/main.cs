using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main : MonoBehaviour {

	// initialize the 2 game objects
	public GameObject linker;
	public GameObject rechter;

	// the 2 scripts
	private rotate_links left;
	private rotate_rechts right;

	// text and time and succes
	public Text winText;
	public Text loseText;
	public Text timeText;
	private float timeLeft = 20.0f;
	private double time;
	bool succes;

	// Use this for initialization
	void Start () {
		winText.enabled = false;
		loseText.enabled = false;
		succes = false;
		setTimeText ();
	}
	
	// Update is called once per frame
	void Update () {
		// get the scripts
		left = linker.GetComponent<rotate_links> ();
		right = rechter.GetComponent<rotate_rechts> ();

		// if finished!
		if (left.finished_left && right.finished_right) {
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
