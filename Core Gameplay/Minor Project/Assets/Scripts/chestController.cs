using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class chestController : NetworkBehaviour {

	public GameObject minigame1Prefab;

	// Use this for initialization
	void Start () {
		Eventmanager.Instance.EventonChestActivated += HandleEventonChestActivated;
	}

	[Server]
	void HandleEventonChestActivated(){
		Gamemanager.Instance.onNextLevelLoad = returnLevel;
		NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		Manager.playerPrefab = minigame1Prefab;
		Manager.ServerChangeScene ("Minigame1");
	}

	void returnLevel(){
		main main = (main)GameObject.FindObjectOfType (typeof(main));
		main.nextlevel = ("BasisLevel");
	}
}
