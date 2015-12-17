using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class networkMenu : MonoBehaviour {
	public Canvas networkMultiplayer;
	public Canvas accountCreate;
	public Canvas play;
	public Canvas loggedIn;

	public void pressLogin(){
		WebManager.Instance.login ();
	}

	public void pressBack(){
		play.enabled = true;
		networkMultiplayer.enabled = false;
	}

	public void pressCreateAccount(){
		accountCreate.enabled = true;
		networkMultiplayer.enabled = false;
	}

	void Update(){
		if (WebManager.Instance.currentUser != null) {
			loggedIn.enabled = true;
			networkMultiplayer.enabled = false;
		}
	}
}