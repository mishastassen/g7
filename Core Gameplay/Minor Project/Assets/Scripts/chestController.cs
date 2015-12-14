using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class chestController : NetworkBehaviour {

	//private Gamemanager gameManager;

	public GameObject minigame1Prefab;
	public string currentLevel;

	private bool eventEnabled;

	// Use this for initialization
	void OnEnable () {
		Eventmanager.Instance.EventonChestActivated += HandleEventonChestActivated;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
		eventEnabled = true;
	}

	void Start() {
		Gamemanager.Instance.currentLevel = "Network manager";
	}
	/*
	void Update() {
		Debug.Log (Gamemanager.Instance.currentLevel);
	}
	*/
	[Server]
	void HandleEventonChestActivated(){
		//Gamemanager.Instance.onNextLevelLoad = returnLevel;
		NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		Manager.playerPrefab = minigame1Prefab;
		Eventmanager.Instance.triggerLevelFinished ("Minigame1");
		//Eventmanager.Instance.triggerMinigameStarted ("Minigame1", currentLevel);
		//gameManager.currentLevel = currentLevel;
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
