using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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


	// Use this for initialization
	void Start () {
		play.enabled = false;
		credits.enabled = false;
		sure.enabled = false;
		help.enabled = false;
		options.enabled = false;
	}

	public void PressPlay (){
		play.enabled = true;
		menu.enabled = false;
	}

	public void PressHelp() {
		help.enabled = true;
		menu.enabled = false;
	}

	public void PressExitHelp (){
		help.enabled = false;
		menu.enabled = true;
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
