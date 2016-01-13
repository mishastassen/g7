using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class connectButton : MonoBehaviour {

	public User linkedUser;
	public WebManager webmanager;

	public GameObject popUpPanel;
	public GameObject inputButtonPanel;

	public Canvas levelSelect;
	public Canvas loggedIn;

	public void onClick(){
		if (webmanager.currentUser != null && webmanager.currentUser.UserId != linkedUser.UserId) {
			levelSelect.gameObject.GetComponent<levelSelectCanvas> ().player1 = webmanager.currentUser;
			levelSelect.gameObject.GetComponent<levelSelectCanvas> ().player2 = linkedUser;
			levelSelect.gameObject.GetComponent<levelSelectCanvas> ().offline = false;
			levelSelect.GetComponent<levelSelectCanvas> ().returnCanvas = loggedIn;
			levelSelect.GetComponent<levelSelectCanvas> ().UpdateButtons ();
			loggedIn.enabled = false;
			levelSelect.enabled = true;
			loggedIn.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
			levelSelect.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
			/*JSONClass messageBody = new JSONClass ();
			messageBody ["reqUserId"].AsInt = webmanager.currentUser.UserId;
			messageBody["reqUsername"] = webmanager.currentUser.Username;
			StartCoroutine (webmanager.IEsendMessage (linkedUser.UserId, "playGame", messageBody));
			popUpPanel.SetActive (true);
			//inputButtonPanel.SetActive (false);
			popUpPanel.GetComponent<messagePopup> ().sendRequest ();*/

		}
	}
}
