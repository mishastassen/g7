using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class networkMenu : MonoBehaviour {
	public Canvas networkMultiplayer;
	public Canvas accountCreate;
	public Canvas play;
	public Canvas loggedIn;

	public InputField loginName;
	public InputField loginPass;

	public void pressLogin(){
		WebManager.Instance.login (loginName.text,loginPass.text);
		networkMultiplayer.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		loggedIn.transform.FindChild ("LogoutButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
		loggedIn.enabled = true;
		networkMultiplayer.enabled = false;
	}

	public void pressBack(){
		play.enabled = true;
		networkMultiplayer.enabled = false;
		networkMultiplayer.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		play.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void pressCreateAccount(){
		accountCreate.enabled = true;
		networkMultiplayer.enabled = false;
		networkMultiplayer.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		accountCreate.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
	}
		
}