using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class escMenu : MonoBehaviour {

	public void pressQuit(){
		//Application.Quit();
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
