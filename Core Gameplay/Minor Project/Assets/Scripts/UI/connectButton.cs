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
		if (webmanager.currentUser != null && webmanager.currentUser.UserId != linkedUser.UserId) {
			JSONClass messageBody = new JSONClass ();
			messageBody ["reqUserId"].AsInt = webmanager.currentUser.UserId;
			messageBody["reqUsername"] = webmanager.currentUser.Username;
			StartCoroutine (webmanager.IEsendMessage (linkedUser.UserId, "playGame", messageBody));
			popUpPanel.SetActive (true);
			//inputButtonPanel.SetActive (false);
			popUpPanel.GetComponent<messagePopup> ().sendRequest ();
		}
	}
}
