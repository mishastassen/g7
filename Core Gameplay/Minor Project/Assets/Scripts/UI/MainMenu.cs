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
	public Canvas levelSelect;
	public Canvas localOnline;

	public NetworkManager networkmanager;
	public WebManager webmanager;

	// Use this for initialization
	void Start () {
		localOnline.enabled = false;
		levelSelect.enabled = false;
		play.enabled = false;
		credits.enabled = false;
		sure.enabled = false;
		help.enabled = false;
		options.enabled = false;
		createAccount.enabled = false;
		network.enabled = false;
		loggedIn.enabled = false;
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void PressPlay (){
		play.enabled = true;
		menu.enabled = false;
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		play.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void PressHelp() {
		help.enabled = true;
		menu.enabled = false;
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		closeHelpButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void PressExitHelp (){
		help.enabled = false;
		menu.enabled = true;
		closeHelpButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void PressOptions() {
		options.enabled = true;
		menu.enabled = false;
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		closeOptionsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void PressExitOptions(){
		menu.enabled = true;
		options.enabled = false;
		closeOptionsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void PressExitGame() {
		sure.enabled = true;
		menu.enabled = false;
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
	}

	public void PressExitGameYes(){
		Application.Quit ();
	}

	public void PressExitGameNo(){
		sure.enabled = false;
		menu.enabled = true;
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}


	public void PressCredits (){
		credits.enabled = true;
		menu.enabled = false;
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		closeCreditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void closeCredits() {
		credits.enabled = false;
		menu.enabled = true;
		closeCreditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", false);
		exitGameButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
		creditsButton.transform.parent.GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void pressLocalGame(){
		networkmanager.StartHost ();
	}
}
