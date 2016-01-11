using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class playMenu : MonoBehaviour {
	public Canvas mainMenu;
	public Canvas networkMultiplayer;
	public Canvas play;

	public void pressLocalGame(){
		WebManager.Instance.localmultiplayer = true;
		NetworkManager networkmanager = (NetworkManager)GameObject.FindObjectOfType (typeof(NetworkManager));
		networkmanager.StartHost ();
	}

	public void pressBack(){
		mainMenu.enabled = true;
		play.enabled = false;
		play.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		mainMenu.transform.FindChild ("ExitGameContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
		mainMenu.transform.FindChild ("CreditsContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void pressNetworkGame(){
		networkMultiplayer.enabled = true;
		play.enabled = false;
		play.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		networkMultiplayer.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
	}
}
