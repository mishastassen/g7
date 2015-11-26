using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Playminigame : NetworkBehaviour {

	public GameObject prefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnButtonClick(){
		NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		Manager.playerPrefab = prefab;
		Manager.ServerChangeScene ("Minigame1");
	}
}
