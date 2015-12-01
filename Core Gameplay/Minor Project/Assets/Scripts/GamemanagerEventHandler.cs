using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GamemanagerEventHandler : NetworkBehaviour {

	public GameObject playerPrefab;
	public GameObject PickUp1Prefab;

	private NetworkManager networkmanager;

	// Use this for initialization
	void Start () {
		Eventmanager.Instance.EventonPlayerDeath += HandleEventonPlayerDeath;
		Eventmanager.Instance.EventonPackageDestroyed += HandleEventonPackageDestroyed;
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;
		networkmanager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
	}

	[Server]
	void HandleEventonLevelFinished (string nextLevel)
	{
		Gamemanager.Instance.packageheld = false;
		networkmanager.ServerChangeScene (nextLevel);
	}

	[Server]
	void HandleEventonPackageDestroyed ()
	{	
		GameObject package = GameObject.FindWithTag ("Package1");
		Transform transform = GameObject.FindWithTag ("PickUp1Spawn").transform;
		Destroy (package);
		GameObject newPackage = (GameObject)Instantiate (PickUp1Prefab, transform.position, transform.rotation);
		NetworkServer.Spawn (newPackage);
	}

	[Server]
	void HandleEventonPlayerDeath (GameObject player)
	{	
		NetworkConnection conn = player.GetComponent<NetworkIdentity> ().connectionToClient;
		short playerControllerId = player.GetComponent<NetworkIdentity> ().playerControllerId;
		Transform transform = GameObject.FindWithTag ("SpawnLocation").transform;
		GameObject newPlayer = (GameObject)Instantiate(playerPrefab, transform.position, transform.rotation);
		NetworkServer.Spawn (newPlayer);
		Destroy (player);
		NetworkServer.ReplacePlayerForConnection (conn, newPlayer, playerControllerId);
	}

}
