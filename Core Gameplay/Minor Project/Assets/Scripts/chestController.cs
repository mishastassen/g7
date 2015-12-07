using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class chestController : NetworkBehaviour {

	public GameObject minigame1Prefab;

	private bool eventEnabled;

	// Use this for initialization
	void OnEnable () {
		Eventmanager.Instance.EventonChestActivated += HandleEventonChestActivated;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
		eventEnabled = true;
	}

	[Server]
	void HandleEventonChestActivated(){
		//Gamemanager.Instance.onNextLevelLoad = returnLevel;
		NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		Manager.playerPrefab = minigame1Prefab;
		Eventmanager.Instance.triggerLevelFinished ("Minigame1");
	}

	void returnLevel(){
		main main = (main)GameObject.FindObjectOfType (typeof(main));
		//main.nextlevel = ("BasisLevel");
	}

	void OnDisable(){
		if (eventEnabled) {
			Eventmanager.Instance.EventonChestActivated -= HandleEventonChestActivated;
			eventEnabled = false;
		}
	}
}
