using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class loggedInMenu : MonoBehaviour {

	public Canvas loggedIn;
	public Canvas network;
	public Canvas mainMenu;
	public Canvas levelSelect;
	public Canvas play;

	public GameObject onlineUserPanel, onlineUserPrefab, popUpPanel;

	public Text usernameText;

	private float time = 0;
	private float time2 = 0;

	public void pressLogOut(){
		WebManager.Instance.logout ();
		loggedIn.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		loggedIn.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		network.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
		loggedIn.enabled = false;
		network.enabled = true;
	}

	public void pressBack(){
		loggedIn.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		loggedIn.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		play.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
		loggedIn.enabled = false;
		play.enabled = true;
	}

	void Update(){
		if (loggedIn.enabled == true && WebManager.Instance.currentUser == null && time > 1) {
			loggedIn.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
			loggedIn.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
			network.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
			loggedIn.enabled = false;
			network.enabled = true;
			time = 0;
		} else if (loggedIn.enabled == true && WebManager.Instance.currentUser == null) {
			time += Time.deltaTime;
		} else if(time != 0) {
			time = 0;
		}
		if (time2 > 0.5) {
			if (loggedIn.enabled && WebManager.Instance.currentUser != null) {
				List<GameObject> oldText = new List<GameObject> ();
				foreach (Transform child in onlineUserPanel.transform)
					oldText.Add (child.gameObject);
				oldText.ForEach (child => Destroy (child));
				foreach (User user in WebManager.Instance.onlineUsersList) {
					GameObject text = Instantiate (onlineUserPrefab) as GameObject;
					text.GetComponent<Text> ().text = user.Username;
					text.GetComponent<Text> ().color = Color.black;
					text.GetComponent<Text> ().fontStyle = FontStyle.Italic;
					text.transform.SetParent (onlineUserPanel.transform, false);
					text.GetComponent<connectButton> ().popUpPanel = popUpPanel;
					text.GetComponent<connectButton> ().webmanager = WebManager.Instance;
					text.GetComponent<connectButton> ().linkedUser = user;
					text.GetComponent<connectButton> ().levelSelect = levelSelect;
					text.GetComponent<connectButton> ().loggedIn = loggedIn;
				}
			}
			time2 = 0;
		} else {
			time2 += Time.deltaTime;
		}
	}
}
