using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class highScoreDisplay : MonoBehaviour {

	public Text spotText;
	public Text highScoreText;

	public void displayHighscore(string spot, string highScore){
		spotText.text = spot;
		highScoreText.text = highScore;
	}

}
