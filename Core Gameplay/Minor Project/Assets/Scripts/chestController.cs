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
		NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		Manager.playerPrefab = minigame1Prefab;
		Manager.ServerChangeScene ("Minigame1");
	}
}
