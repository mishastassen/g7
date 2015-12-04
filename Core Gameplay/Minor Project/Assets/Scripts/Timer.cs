using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
	
	public Text timerText;
	private float timer;
	private float minutes;
	private string seconds;
	private bool levelFinished;
	
	void Start () {
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;
		timerText.text = "";
		minutes = 0.0f;
		seconds = "";
		levelFinished = false;
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
