using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class AlarmController : NetworkBehaviour {

	//public Text alarmText;
	private float alarmPercent;
	private string alarm;
	private bool finishedIncrease;
	private bool finishedDecrease;

	void Start () {
		Eventmanager.Instance.EventonPlayerSpotted += HandleEventonPlayerSpotted;
		Eventmanager.Instance.EventonNoPlayerSpotted += HandleEventonNoPlayerSpotted;
		//Eventmanager.Instance.EventonUpdateAlarm += HandleEventonUpdateAlarm;
		alarmPercent = 0;
		alarm = "Alarm: " + alarmPercent + "%";
		//alarmText.text = "Alarm: " + alarmPercent + "%";
		finishedIncrease = true;
		finishedDecrease = true;
	}
	/*
	void HandleEventonUpdateAlarm (string alarmString) {
		alarmText.text = alarmString;
	}
	*/
	void Update () {
		alarm = "Alarm: " + alarmPercent + "%";
		if (isServer) {
			Eventmanager.Instance.triggerUpdateAlarm (alarm);
		}
	}
	
	void HandleEventonPlayerSpotted() {
		if (finishedIncrease) {
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
		yield return new WaitForSeconds (1);
		finishedIncrease = true;
	}

	IEnumerator decreaseAlarm() {
		alarmPercent -= 5;
		yield return new WaitForSeconds (2);
		finishedDecrease = true;
	}

}
