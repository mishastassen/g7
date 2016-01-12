using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlarmController : MonoBehaviour {

	//public Text alarmText;
	public Scrollbar alarmBar;
	private float alarmPercent;
	private bool finishedIncrease;
	private bool finishedDecrease;

	private bool enabled;

	void Start () {
		alarmBar.size = 0.0f; 
		alarmPercent = 0;
		//alarmText.text = "Alarm: " + alarmPercent + "%";
		finishedIncrease = true;
		finishedDecrease = true;
		Eventmanager.Instance.EventonPlayerSpotted += HandleEventonPlayerSpotted;
		Eventmanager.Instance.EventonNoPlayerSpotted += HandleEventonNoPlayerSpotted;
		enabled = true;
	}			

	void Update () {		
		/*
		if (Gamevariables.alarmPercent == -1) {
			alarmPercent = 0;
			Gamevariables.alarmPercent = 0;
		}
		Gamevariables.alarmPercent = alarmPercent;
		*/
	}

	void OnEnable() {
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
	}

	void OnDisable() {
		if (enabled) {
			Eventmanager.Instance.EventonPlayerSpotted -= HandleEventonPlayerSpotted;
			Eventmanager.Instance.EventonNoPlayerSpotted -= HandleEventonNoPlayerSpotted;
			enabled = false;
		}
	}

	void HandleEventonPlayerSpotted() {
		if (finishedIncrease && alarmPercent != 100) {
			StartCoroutine (increaseAlarm ());
			finishedIncrease = false;
		}
	}

	void HandleEventonNoPlayerSpotted() {
		if (finishedDecrease && alarmPercent != 0) {
			StartCoroutine (decreaseAlarm());
			finishedDecrease = false;
		}
	}

	IEnumerator increaseAlarm() {
		alarmPercent += 5;
		if (alarmPercent >= 100) {
			alarmPercent = 100;
		}
		//alarmText.text = "Alarm: " + alarmPercent + "%";
		alarmBar.size = alarmPercent / 100f;
		if (alarmPercent == 100) {
			//alarmText.color = Color.red;
		}
		yield return new WaitForSeconds (0.5f);
		finishedIncrease = true;
	}

	IEnumerator decreaseAlarm() {
		if (alarmPercent != 100) {
			alarmPercent -= 5;
		}
		//alarmText.text = "Alarm: " + alarmPercent + "%";
		alarmBar.size = alarmPercent / 100f;
		yield return new WaitForSeconds (1);
		finishedDecrease = true;
	}

}
