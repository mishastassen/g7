using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour {

	public Canvas menu;

	public Button playButton;
	public Button helpButton;
	public Button optionsButton;
	public Button creditsButton;
	public Button exitGameButton;

	public Canvas play;
	public Text playText;
	public Button localButton;
	public Button networkButton;

	public Canvas credits;
	public Text creditText;
	public Button closeCreditsButton;

	public Canvas sure;
	public Button exitYes;
	public Button exitNo;

	public Canvas help;
	public Button closeHelpButton;

	public Canvas options;
	public Button closeOptionsButton;

	public Canvas createAccount;
	public Canvas network;
	public Canvas loggedIn;

	public NetworkManager networkmanager;
	public WebManager webmanager;

	// Use this for initialization
	void Start () {
		play.enabled = false;
		credits.enabled = false;
		sure.enabled = false;
		help.enabled = false;
		options.enabled = false;
		createAccount.enabled = false;
		network.enabled = false;
		loggedIn.enabled = false;
	}

	public void PressPlay (){
		play.enabled = true;
		menu.enabled = false;
	}

	public void PressHelp() {
		help.enabled = true;
		menu.enabled = false;
		closeHelpButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void PressExitHelp (){
		help.enabled = false;
		menu.enabled = true;
		closeHelpButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
	}

	public void PressOptions() {
		options.enabled = true;
		menu.enabled = false;
	}

	public void PressExitOptions(){
		menu.enabled = true;
		options.enabled = false;
	}

	public void PressExitGame() {
		sure.enabled = true;
		menu.enabled = false;
	}

	public void PressExitGameYes(){
		Application.Quit ();
	}

	public void PressExitGameNo(){
		sure.enabled = false;
		menu.enabled = true;
	}


	public void PressCredits (){
		credits.enabled = true;
		menu.enabled = false;
		}

	public void closeCredits() {
		credits.enabled = false;
		menu.enabled = true;
	}

	public void pressLocalGame(){
		networkmanager.StartHost ();
	}
}
