using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class levelSelectCanvas : MonoBehaviour {

	public User player1;
	public User player2;
	public List<GameObject> levelButtons;
	[HideInInspector]
	public bool offline;

	public GameObject popUpPanel;
	public Canvas levelSelect;
	public Canvas returnCanvas;
	public Canvas localOnlineCanvas;

	public void UpdateButtons(){
		foreach (GameObject button in levelButtons) {
			button.GetComponent<levelSelectButton> ().updateButton ();
		}
	}

	public void onLevelSelect(string levelName){
		WebManager.Instance.level1 = levelName;
		if (offline) {
			gameObject.GetComponent<Canvas> ().enabled = false;
			localOnlineCanvas.enabled = true;
		} else {
			JSONClass messageBody = new JSONClass ();
			messageBody ["reqUserId"].AsInt = WebManager.Instance.currentUser.UserId;
			messageBody["reqUsername"] = WebManager.Instance.currentUser.Username;
			StartCoroutine (WebManager.Instance.IEsendMessage (player2.UserId, "playGame", messageBody));
			popUpPanel.SetActive (true);
			popUpPanel.GetComponent<messagePopup> ().sendRequest ();
			gameObject.GetComponent<Canvas> ().enabled = false;
			returnCanvas.enabled = true;
			levelSelect.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
			returnCanvas.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
		}
	}

	public void goBack(){
		gameObject.GetComponent<Canvas> ().enabled = false;
		returnCanvas.enabled = true;
		levelSelect.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		returnCanvas.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
	}
}
