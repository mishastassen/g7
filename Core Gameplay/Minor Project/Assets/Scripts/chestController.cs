using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class chestController : NetworkBehaviour {

	public GameObject minigame1Prefab;
	//public GameObject startLocation;
	public string currentLevel;
	public int difficulty;

	private bool packageNearby;
	private int playerCount;

	private bool eventEnabled;

	// animotor  and particle system stuff
	private Animator anim;

	// Use this for initialization
	void OnEnable () {
		anim = GetComponent<Animator> ();
	}

	void Start() {
		playerCount = 0;
		Gamevariables.returnLevel = currentLevel;
		Gamevariables.minigameDifficulty = difficulty;
		Eventmanager.Instance.EventonChestActivated += HandleEventonChestActivated;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
		eventEnabled = true;
	}

	[Server]
	void HandleEventonChestActivated(){
		if (packageNearby && playerCount == 2) {
			//Gamemanager.Instance.onNextLevelLoad = returnLevel;
			NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager> ();
			Manager.playerPrefab = minigame1Prefab;
			Eventmanager.Instance.triggerLevelSwitch ("Minigame1");
			//Eventmanager.Instance.triggerMinigameStarted ();
		}
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

	void Update() {
		//Debug.Log (playerCount);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "PickUp1" || other.tag == "PickUpMagic") {
			packageNearby = true;
		}
		if (other.tag == "Player") {
			playerCount += 1;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "PickUp1" || other.tag == "PickUpMagic") {
			packageNearby = false;
		}
		if (other.tag == "Player") {
			playerCount -= 1;
		}
	}
}
