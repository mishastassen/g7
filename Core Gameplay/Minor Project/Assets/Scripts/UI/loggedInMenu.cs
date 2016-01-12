using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class loggedInMenu : MonoBehaviour {

	public Canvas loggedIn;
	public Canvas network;
	public Canvas mainMenu;

	public Text usernameText;

	public float time = 0;

	public void pressLogOut(){
		WebManager.Instance.logout ();
		loggedIn.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		network.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
		loggedIn.enabled = false;
		network.enabled = true;
	}

	void Update(){
		if (loggedIn.enabled == true && WebManager.Instance.currentUser == null && time > 1) {
			loggedIn.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
			network.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
			loggedIn.enabled = false;
			network.enabled = true;
			time = 0;
		} else if (loggedIn.enabled == true && WebManager.Instance.currentUser == null) {
			time += Time.deltaTime;
		} else if(time != 0) {
			time = 0;
		}
	}
}
