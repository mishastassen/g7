using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class highScoreDisplay : MonoBehaviour {

	public Text spotText;
	public Text highScoreText;
	public Text timeText;

	public void displayHighscore(string spot, string players, string score){
		spotText.text = spot;
		highScoreText.text = players;
		timeText.text = score;
	}

}
