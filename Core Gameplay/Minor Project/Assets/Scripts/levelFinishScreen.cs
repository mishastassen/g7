using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;

public class levelFinishScreen : NetworkBehaviour {

	[HideInInspector]
	public string nextLevel;
	public GameObject highscorePanel;
	public GameObject highscoreEntryPrefab;


	void OnEnable(){
		int highscore = (int)Mathf.Round (Gamevariables.timer);
		int levelId = GameObject.Find("LevelManager").GetComponent<Levelvariables>().levelId;
		if (WebManager.Instance.currentUser != null && NetworkServer.active) {
			WebManager.Instance.updateHighscores (levelId, highscore);
		}
		WebManager.Instance.getHighscores (levelId, this);
	}

	public void nextLevelButton(){
		Eventmanager.Instance.triggerLevelSwitch (nextLevel);
	}

	public void displayHighscores(JSONNode highscores){
		JSONNode top10 = highscores ["top10"];
		JSONNode bestTime = highscores ["bestTime"][0];
		foreach( var key in top10.Keys )
		{
			string highscoreText = highscores ["top10"] [key] ["Player1"] + " and " + highscores ["top10"] [key] ["Player2"] + " in " + highscores ["top10"] [key] ["Highscore"] +" seconds" ;
			GameObject highscoreEntry = Instantiate (highscoreEntryPrefab) as GameObject;
			highscoreEntry.transform.SetParent (highscorePanel.transform, false);
			string spot = (string)key;
			highscoreEntry.GetComponent<highScoreDisplay> ().displayHighscore (spot, highscoreText);
		}

	}
}
