using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;

public class levelFinishScreen : NetworkBehaviour {

	[HideInInspector]
	public string nextLevel;

	void OnEnable(){
		Debug.Log ("level finish screen enabled");
		int highscore = (int)Mathf.Round (Gamevariables.timer);
		int levelId = GameObject.Find("LevelManager").GetComponent<Levelvariables>().levelId;
		if (WebManager.Instance.currentUser != null && NetworkServer.active) {
			Debug.Log ("screen update highscore");
			WebManager.Instance.updateHighscores (levelId, highscore);
		}
	}

	public void nextLevelButton(){
		Eventmanager.Instance.triggerLevelSwitch (nextLevel);
	}

	public void displayHighscores(JSONNode highscores){
		
	}
}
