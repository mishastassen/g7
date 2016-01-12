using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelSelectButton : MonoBehaviour {

	public levelSelectCanvas levelSelect;

	public int levelId;
	public string levelName;

	public void updateButton(){
		if (levelSelect.offline) {
			if(PlayerPrefs.HasKey("levelProgress")){
				Debug.Log (PlayerPrefs.GetInt ("levelProgress"));
				if (PlayerPrefs.GetInt ("levelProgress") + 1 >= levelId) {
					gameObject.GetComponent<Button> ().interactable = true;
				} else {
					gameObject.GetComponent<Button> ().interactable = false;
				}
			}else if(levelId == 1){
				gameObject.GetComponent<Button> ().interactable = true;
			}else{
				gameObject.GetComponent<Button> ().interactable = false;
			}
		} else {
			if (levelSelect.player1.levelProgress+1 >= levelId && levelSelect.player2.levelProgress+1 >= levelId) {
				gameObject.GetComponent<Button> ().interactable = true;
			} else {
				gameObject.GetComponent<Button> ().interactable = false;
			}
		}
	}

	public void onClick(){
		levelSelect.onLevelSelect (levelName);
	}
		
}