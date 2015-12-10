using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class AlarmEventHandler : NetworkBehaviour {

	public Text alarmText;

	void Start () {
		Eventmanager.Instance.EventonUpdateAlarm += HandleEventonUpdateAlarm;
	}
	
	void Update () {
	
	}

	void HandleEventonUpdateAlarm (string alarmString) {
		 alarmText.text = alarmString;
	}
}
