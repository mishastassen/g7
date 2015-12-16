﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class messagePopup : MonoBehaviour {

	public bool requestedgame = false;
	public GameObject yesButton, noButton, inputPanel, waitText, messageText, okayButton;
	string state = "open";
	int reqUserId;

	public WebManager webmanager;
	public NetworkManager networkmanager;

	public void readMessage(JSONNode message){
		if (message ["Type"].Value == "playGame" && state != "open") {
			int busyUserId = message ["messageBody"] ["reqUserId"].AsInt;
			JSONClass messageBody = new JSONClass ();
			messageBody ["acceptUserId"].AsInt = webmanager.currentUser.UserId;
			StartCoroutine (webmanager.IEsendMessage (busyUserId, "userBusy", messageBody));
		} else {
			messageText.GetComponent<Text> ().text = "";
			yesButton.SetActive (false);
			noButton.SetActive (false);
			waitText.SetActive (false);
			okayButton.SetActive (false);
			if (message ["Type"].Value == "playGame" && state == "open") {
				reqUserId = message ["messageBody"] ["reqUserId"].AsInt;
				string username = message ["messageBody"] ["reqUsername"].Value;
				StartCoroutine (otherRequestsGame (username));
			} else if (message ["Type"].Value == "acceptGame" && state == "gameRequested") {
				reqUserId = message ["messageBody"] ["acceptUserId"].AsInt;
				startHost ();
			} else if (message ["Type"].Value == "HostStarted" && state == "gameAccepted") {
				state = "open";
				Debug.Log ("starting");
				Debug.Log (message ["messageBody"] ["gameIp"]);
				networkmanager.networkAddress = message ["messageBody"] ["gameIp"];
				networkmanager.StartClient ();
			} else if (message ["Type"].Value == "denyGame" && state == "gameRequested") {
				reqUserId = message ["messageBody"] ["acceptUserId"].AsInt;
				gameDenied ();
			} else if (message ["Type"].Value == "userBusy" && state == "gameRequested") {
				reqUserId = message ["messageBody"] ["acceptUserId"].AsInt;
				userBusy ();
			} else {
				Debug.LogError ("Wrong message received");
				inputPanel.SetActive (true);
				gameObject.SetActive (false);
			}
		}
	}

	IEnumerator sendGameRequest(){
		messageText.GetComponent<Text> ().text = "Waiting for response";
		waitText.SetActive (true);
		state = "gameRequested";
		yield return new WaitForSeconds(30.0f);
		if (state == "gameRequested") {
			Debug.Log ("no response from user");
			closePopup ();
		}
	}

	IEnumerator otherRequestsGame(string username){
		yesButton.SetActive(true);
		noButton.SetActive(true);
		messageText.GetComponent<Text> ().text = "User " + username + " requested game, would you like to accept?";
		state = "otherRequestedGame";
		yield return new WaitForSeconds(30.0f);
		if (state == "otherRequestedGame") {
			sendGameDenied ();
		}
	}

	IEnumerator weAcceptedGame(){
		yesButton.SetActive (false);
		noButton.SetActive (false);
		waitText.SetActive (true);
		messageText.GetComponent<Text> ().text = "Waiting for host to start";
		state = "gameAccepted";
		yield return new WaitForSeconds(30.0f);
		if (state == "gameAccepted") {
			Debug.Log ("no response from host");
			closePopup ();
		}
	}

	public void sendGameAccepted(){
		JSONClass messageBody = new JSONClass ();
		messageBody ["acceptUserId"].AsInt = webmanager.currentUser.UserId;
		StartCoroutine (webmanager.IEsendMessage (reqUserId, "acceptGame", messageBody));
		StartCoroutine (weAcceptedGame ());
	}

	public void sendRequest(){
		messageText.GetComponent<Text> ().text = "";
		yesButton.SetActive(false);
		noButton.SetActive(false);
		waitText.SetActive (false);
		okayButton.SetActive (false);
		StartCoroutine (sendGameRequest());
	}

	public void sendGameDenied(){
		JSONClass messageBody = new JSONClass ();
		messageBody ["acceptUserId"].AsInt = webmanager.currentUser.UserId;
		StartCoroutine (webmanager.IEsendMessage (reqUserId, "denyGame", messageBody));
		state = "open";
		inputPanel.SetActive (true);
		gameObject.SetActive (false);
	}

	public void gameDenied(){
		state = "open";
		waitText.SetActive (false);
		inputPanel.SetActive (true);
		gameObject.SetActive (false);
	}

	public void startHost(){
		JSONClass messageBody = new JSONClass ();
		messageBody ["gameIp"] = webmanager.currentUser.Ip;
		Debug.Log (messageBody.ToString ());
		StartCoroutine (webmanager.IEsendMessage (reqUserId, "HostStarted", messageBody));
		networkmanager.StartHost ();
	}

	public void userBusy(){
		state = "okayButton";
		waitText.SetActive (false);
		messageText.GetComponent<Text> ().text = "User is busy, try again in a few minutes";
		okayButton.SetActive (true);
	}

	public void closePopup(){
		state = "open";
		inputPanel.SetActive (true);
		gameObject.SetActive (false);
	}
}