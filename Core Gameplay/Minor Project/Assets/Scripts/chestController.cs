using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class chestController : NetworkBehaviour {

	public GameObject minigame1Prefab;

	// Use this for initialization
	void OnEnable () {
		Eventmanager.Instance.EventonChestActivated += HandleEventonChestActivated;
	}

	[Server]
	void HandleEventonChestActivated(){
		Gamemanager.Instance.onNextLevelLoad = returnLevel;
		PlayerEventHandler[] handlers = GameObject.FindObjectsOfType<PlayerEventHandler>();
		foreach(PlayerEventHandler handler in handlers){
			handler.Disable ();
		}
		NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		Manager.playerPrefab = minigame1Prefab;
		Manager.ServerChangeScene ("Minigame1");
	}

	void returnLevel(){
		main main = (main)GameObject.FindObjectOfType (typeof(main));
		//main.nextlevel = ("BasisLevel");
	}
}
