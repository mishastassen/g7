using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;

public class levelFinishScreen : NetworkBehaviour {

	[HideInInspector]
	public string nextLevel;

	void OnEnable(){
		int highscore = (int)Mathf.Round (Gamevariables.timer);
		int levelId = GameObject.Find("LevelManager").GetComponent<Levelvariables>().levelId;
		if (WebManager.Instance.currentUser != null && isServer) {
			WebManager.Instance.updateHighscores (levelId, highscore);
		}
	}

	public void nextLevelButton(){
		Eventmanager.Instance.triggerLevelSwitch (nextLevel);
	}

	public void displayHighscores(JSONNode highscores){
		
	}
}
