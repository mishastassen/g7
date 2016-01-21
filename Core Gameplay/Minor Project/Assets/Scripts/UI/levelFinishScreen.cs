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
		if (WebManager.Instance.currentUser != null) {
			if (NetworkServer.active) {
				WebManager.Instance.updateHighscores (levelId, highscore, this);
			}
			if (WebManager.Instance.currentUser.levelProgress < levelId) {
				WebManager.Instance.updateLevelProgress (levelId);
			}
		} else {
			string scoreText = "";
			string highscoreText = "Login to see highscores";
			GameObject highscoreEntry = Instantiate (highscoreEntryPrefab) as GameObject;
			highscoreEntry.transform.SetParent (highscorePanel.transform, false);
			string spot = "";
			highscoreEntry.GetComponent<highScoreDisplay> ().displayHighscore (spot, highscoreText, scoreText);
		}
		if (PlayerPrefs.GetInt("levelProgress") == null || PlayerPrefs.GetInt("levelProgress") < levelId) {
			PlayerPrefs.SetInt ("levelProgress", levelId);
		}
		headline.text = "You finished the level in " + highscore + " seconds!";
	}

	public void nextLevelButton(){
		Eventmanager.Instance.triggerLevelSwitch (nextLevel);
	}

	public void updatedHighscores(string result){
		int levelId = GameObject.Find("LevelManager").GetComponent<Levelvariables>().levelId;
		WebManager.Instance.getHighscores (levelId, this);
	}

	public void displayHighscores(JSONNode highscores){
		JSONNode top10 = highscores ["top10"];
		JSONNode bestTime = highscores ["bestTime"][0];
		for (int i = 0; i<5; i++)
		{
			if (top10 [i] != null) {
				string highscoreText = top10 [i] ["Player1"] + "\n" + top10 [i] ["Player2"];
				string scoreText = top10 [i] ["Highscore"] + " seconds";
				GameObject highscoreEntry = Instantiate (highscoreEntryPrefab) as GameObject;
				highscoreEntry.transform.SetParent (highscorePanel.transform, false);
				string spot = (i+1).ToString ();
				highscoreEntry.GetComponent<highScoreDisplay> ().displayHighscore (spot, highscoreText, scoreText);
			}
		}
		string personalscoreText = bestTime ["Player1"] + "\n" + bestTime ["Player2"];
		string bestScoreText = bestTime["Highscore"] + " seconds";
		string personalSpot = bestTime["position"];
		GameObject personalscoreEntry = Instantiate (highscoreEntryPrefab) as GameObject;
		personalscoreEntry.transform.SetParent (personalscorePanel.transform, false);
		personalscoreEntry.GetComponent<highScoreDisplay> ().displayHighscore (personalSpot, personalscoreText, bestScoreText);
	}
}
