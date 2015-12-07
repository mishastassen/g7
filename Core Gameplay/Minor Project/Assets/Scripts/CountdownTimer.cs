using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour {
	
	public Text timerText;
	public float startTime;
	private float timer;
	private float time;
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
		time = startTime - timer;
		minutes = Mathf.Floor (time / 60);
		seconds = (time % 60).ToString ("00");
		timerText.text = minutes + ":" + seconds;
	}
}
