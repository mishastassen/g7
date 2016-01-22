using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class minigame3Start : NetworkBehaviour {

	private bool eventEnabled;
	private bool packageNearby;
	private int playerCount;

	public string minigame3SceneName;

	public GameObject minigame3Player;

	void Start(){
		Eventmanager.Instance.EventonMinigame3Activated += HandleEventonMinigame3Activated;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
		eventEnabled = true;
		playerCount = 0;
	}

	void OnDisable(){
		if (eventEnabled) {
			Eventmanager.Instance.EventonMinigame3Activated -= HandleEventonMinigame3Activated;
			eventEnabled = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			playerCount += 1;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			playerCount -= 1;
		}
	}

	[Server]
	void HandleEventonMinigame3Activated(){
		if (playerCount >= 2) {
			GameNetworkManager.singleton.playerPrefab = minigame3Player;
			Eventmanager.Instance.triggerLevelSwitch (minigame3SceneName);
		}
	}

   
}
