using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;

public class levelFinishScreen : MonoBehaviour {

	[HideInInspector]
	public string nextLevel;
	public GameObject highscorePanel;
	public GameObject personalscorePanel;
	public GameObject highscoreEntryPrefab;
	public Text headline;


	void OnEnable(){
		int highscore = (int)Mathf.Round (Gamevariables.timer);
		int levelId = GameObject.Find("LevelManager").GetComponent<Levelvariables>().levelId;
		if (WebManager.Instance.currentUser != null && NetworkServer.active) {
			WebManager.Instance.updateHighscores (levelId, highscore);
			WebManager.Instance.getHighscores (levelId, this);
			if (WebManager.Instance.currentUser.levelProgress < levelId) {
				WebManager.Instance.updateLevelProgress (levelId);
			}
		}
		if (PlayerPrefs.GetInt("levelProgress") == null || PlayerPrefs.GetInt("levelProgress") < levelId) {
			PlayerPrefs.SetInt ("levelProgress", levelId);
		}
		headline.text = "You finished the level in " + highscore + " seconds!";
	}

	public void nextLevelButton(){
		Eventmanager.Instance.triggerLevelSwitch (nextLevel);
	}

	public void displayHighscores(JSONNode highscores){
		JSONNode top10 = highscores ["top10"];
		JSONNode bestTime = highscores ["bestTime"][0];
		for (int i = 0; i<10; i++)
		{
			if (top10 [i] != null) {
				string highscoreText = top10 [i] ["Player1"] + " and " + top10 [i] ["Player2"] + " in " + top10 [i] ["Highscore"] + " seconds";
				GameObject highscoreEntry = Instantiate (highscoreEntryPrefab) as GameObject;
				highscoreEntry.transform.SetParent (highscorePanel.transform, false);
				string spot = (i+1).ToString ();
				highscoreEntry.GetComponent<highScoreDisplay> ().displayHighscore (spot, highscoreText);
			}
		}
		string personalscoreText = bestTime ["Player1"] + " and " + bestTime["Player2"] + " in " + bestTime["Highscore"] + " seconds";
		string personalSpot = bestTime["position"];
		GameObject personalscoreEntry = Instantiate (highscoreEntryPrefab) as GameObject;
		personalscoreEntry.transform.SetParent (personalscorePanel.transform, false);
		personalscoreEntry.GetComponent<highScoreDisplay> ().displayHighscore (personalSpot, personalscoreText);
	}
}
