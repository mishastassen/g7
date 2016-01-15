using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class AlarmController : NetworkBehaviour {

	//public Text alarmText;
	public Scrollbar alarmBar;
	[SyncVar]
	private float alarmPercent;
	private bool finishedIncrease;
	private bool finishedDecrease;
	public bool spotted = false;

	private bool enabled;

	void Start () {
		alarmBar.size = 0.0f; 
		alarmPercent = 0;
		//alarmText.text = "Alarm: " + alarmPercent + "%";
		finishedIncrease = true;
		finishedDecrease = true;
		if (isServer) {
			Eventmanager.Instance.EventonPlayerSpotted += HandleEventonPlayerSpotted;
			Eventmanager.Instance.EventonNoPlayerSpotted += HandleEventonNoPlayerSpotted;
			Gamemanager.Instance.onDisableEventHandlers += OnDisable;
			enabled = true;
		}
	}			

	void Update(){
		alarmBar.size = alarmPercent / 100f;
	}

	void OnDisable() {
		if (isServer) {
			if (enabled) {
				Eventmanager.Instance.EventonPlayerSpotted -= HandleEventonPlayerSpotted;
				Eventmanager.Instance.EventonNoPlayerSpotted -= HandleEventonNoPlayerSpotted;
				enabled = false;
			}
		}
	}

	void HandleEventonPlayerSpotted() {
		spotted = true;
		if (finishedIncrease && alarmPercent != 100) {
			StartCoroutine (increaseAlarm ());
			finishedIncrease = false;
		}
	}

	void HandleEventonNoPlayerSpotted() {
		spotted = false;
		if (finishedDecrease && alarmPercent != 0) {
			StartCoroutine (decreaseAlarm ());
			finishedDecrease = false;
		}
	}

	IEnumerator increaseAlarm() {
		alarmPercent += 5;
		if (alarmPercent >= 100) {
			alarmPercent = 100;
		}
		//alarmText.text = "Alarm: " + alarmPercent + "%";
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
		yield return new WaitForSeconds (1);
		finishedDecrease = true;
	}

}
