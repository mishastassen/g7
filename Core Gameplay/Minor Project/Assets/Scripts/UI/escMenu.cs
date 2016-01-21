using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class escMenu : MonoBehaviour {

	public GameObject standardPlayerPrefab;

	public void pressQuit(){
		GameNetworkManager.singleton.playerPrefab = standardPlayerPrefab;
		Gamemanager.Instance.triggerDisableEventHandlers();
		GameNetworkManager.singleton.StopHost();
	}

	public void pressCloseMenu(){
		this.gameObject.SetActive (false);
	}

	public void pressRestartLevel(){
		Eventmanager.Instance.triggerLevelSwitch (Application.loadedLevelName);
	}

	public void pressResetToLastCheckpoint(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			if (player.GetComponent<NetworkIdentity> ().isLocalPlayer) {
				player.GetComponent<PlayerController>().CmdResetToLastCheckpoint ();
			}
		}
	}
}
