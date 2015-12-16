using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class connectButton : MonoBehaviour {

	public User linkedUser;
	public WebManager webmanager;

	public GameObject popUpPanel;
	public GameObject inputButtonPanel;

	public void onClick(){
		if (webmanager.currentUser != null) {
			JSONClass messageBody = new JSONClass ();
			messageBody ["reqUserId"].AsInt = webmanager.currentUser.UserId;
			messageBody["reqUsername"].Value = webmanager.currentUser.Username;
			StartCoroutine (webmanager.IEsendMessage (linkedUser.UserId, "playGame", messageBody));
			GameObject popUpPanel = GameObject.FindGameObjectWithTag("popUpPanel");
			popUpPanel.SetActive (true);
			inputButtonPanel.SetActive (false);
			popUpPanel.GetComponent<messagePopup> ().sendRequest ();
		}
	}
}
