using UnityEngine;
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
}
