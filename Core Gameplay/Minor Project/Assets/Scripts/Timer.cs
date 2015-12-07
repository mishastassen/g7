using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class Timer : NetworkBehaviour {
	
	public Text timerText;
	private float timer;
	private float minutes;
	private string seconds;
	private bool levelFinished;
	
	void Start () {
		timerText.text = "";
		minutes = 0.0f;
		seconds = "";
		levelFinished = false;
	}

	void OnEnable() {
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;
	}
	
	void Update () {
		if (!levelFinished) {
			UpdateTimer ();
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
