using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
	
	public Text timerText;
	private float timer;
	private float minutes;
	private string seconds;
	private bool levelFinished;

	private bool enabled;
	
	void Start () {
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
