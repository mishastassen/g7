using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class TimerB : MonoBehaviour {
	
	public Text timerText;
	private float timer;
	private float minutes;
	private string seconds;
	private bool levelFinished;
	
	private bool enabled;
	
	void Start () {
		timer = Gamevariables.timer;
		timerText.text = "";
		minutes = 0.0f;
		seconds = "";
		levelFinished = false;
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;
		enabled = true;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
	}

	void OnDisable() {
		if (enabled) {
			Eventmanager.Instance.EventonLevelFinished -= HandleEventonLevelFinished;
			enabled = false;
		}
	}
	
	void Update () {
		if (!levelFinished) {
			UpdateTimer ();
			Gamevariables.timer = timer;
		} else {
			/*
			Analytics.CustomEvent ("Level Finished", new Dictionary<string, object> {
				{ "Levelname", Gamevariables.currentLevel },
				{ "Time", timer }
			});
			*/
			Gamevariables.timer = 0;
		}
	}
	
	void HandleEventonLevelFinished (string nextLevel)
	{	
		levelFinished = true;
	}
	
	void UpdateTimer() {
		timer += Time.deltaTime;
		minutes = Mathf.Floor (timer / 60);
		seconds = (timer % 60).ToString ("00");
		timerText.text = minutes + ":" + seconds;
	}
}
