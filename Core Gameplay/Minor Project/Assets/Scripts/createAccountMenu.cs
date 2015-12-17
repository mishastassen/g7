using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class createAccountMenu : MonoBehaviour {
	public Canvas networkMultiplayer;
	public Canvas accountCreate;

	public void pressBack(){
		networkMultiplayer.enabled = true;
		accountCreate.enabled = false;
	}

	public void pressCreateAccount(){

	}
}