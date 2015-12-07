using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class GamemanagerEventHandler : NetworkBehaviour {

	public GameObject playerPrefab;
	public GameObject PickUp1Prefab;
	
	private NetworkManager networkmanager;
	private NetworkClient m_client;

	private bool levelEnding = false;
	private bool clientEndLevelReady = false;
	const short ClientReadyMsg = 1002;

	// Use this for initialization
	void Start () {
		Eventmanager.Instance.EventonPlayerDeath += HandleEventonPlayerDeath;
		Eventmanager.Instance.EventonPackageDestroyed += HandleEventonPackageDestroyed;
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;
		networkmanager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		NetworkServer.RegisterHandler(ClientReadyMsg, onClientReadyMsg);
		m_client = networkmanager.client;
	}

	void HandleEventonLevelFinished (string nextLevel)
	{	
		if (!levelEnding) {
			levelEnding = true;
			Gamemanager.Instance.triggerDisableEventHandlers ();
			if(!isServer){
				levelEnding = false;
				SendClientReadyMsg();
			}
			if(Gamemanager.Instance.localmultiplayer){
				clientEndLevelReady = true;
			}
			if (isServer) {
				Gamemanager.Instance.packageheld = false;
				StartCoroutine (endLevel (nextLevel));
			}
		}
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

	IEnumerator endLevel(string nextLevel){
		while (!clientEndLevelReady) {
			yield return null;
		}
		levelEnding = false;
		clientEndLevelReady = false;
		networkmanager.ServerChangeScene (nextLevel);
	}


	void SendClientReadyMsg(){
		var msg = new IntegerMessage(1);
		m_client.Send (ClientReadyMsg, msg);
	}

	void onClientReadyMsg(NetworkMessage netMsg){
		clientEndLevelReady = true;
	}
}
