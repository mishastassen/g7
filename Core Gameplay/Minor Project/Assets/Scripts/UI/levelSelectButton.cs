using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelSelectButton : MonoBehaviour {

	public levelSelectCanvas levelSelect;

	public int levelId;
	public string levelName;

	public void updateButton(){
		Debug.Log ("updating buttons");
		if (levelSelect.offline) {
			if (levelId == 1 || (PlayerPrefs.GetInt ("levelProgress") != null && PlayerPrefs.GetInt ("levelProgress") + 1 >= levelId)) {
				gameObject.GetComponent<Button> ().enabled = true;
			} else {
				gameObject.GetComponent<Button> ().enabled = false;

			}
		} else {
			if (levelSelect.player1.levelProgress+1 >= levelId && levelSelect.player2.levelProgress+1 >= levelId) {
				gameObject.GetComponent<Button> ().enabled = true;
			} else {
				gameObject.GetComponent<Button> ().enabled = false;
			}
		}
	}

	public void onClick(){
		levelSelect.onLevelSelect (levelName);
	}
		
}