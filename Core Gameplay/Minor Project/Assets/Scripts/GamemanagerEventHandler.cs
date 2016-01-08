using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityAnalyticsHeatmap;

public class GamemanagerEventHandler : NetworkBehaviour {

	public GameObject playerPrefab;
	public GameObject PickUp1Prefab;
	public GameObject PickUpMagicPrefab;
	
	private NetworkManager networkmanager;
	private NetworkClient m_client;

	private bool levelEnding = false;
	private bool clientEndLevelReady = false;
	const short ClientReadyMsg = 1002;

	// Use this for initialization
	void Start () {
		Eventmanager.Instance.EventonPlayerDeath += HandleEventonPlayerDeath;
		Eventmanager.Instance.EventonPackageDestroyed += HandleEventonPackageDestroyed;
		Eventmanager.Instance.EventonLevelSwitch += HandleEventonLevelSwitch;
		Eventmanager.Instance.EventonCheckpointReached += HandleEventonCheckpointReached;
		Eventmanager.Instance.EventonLevelFinished += HandleEventonLevelFinished;

		networkmanager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		NetworkServer.RegisterHandler(ClientReadyMsg, onClientReadyMsg);
		m_client = networkmanager.client;
	}

	[Server]
	void HandleEventonCheckpointReached (int checkpointNum)
	{
		Gamemanager.Instance.CheckpointReached = checkpointNum;
	}

	void HandleEventonLevelFinished(string nextLevel){
		Time.timeScale = 0;
		GameObject panel = GameObject.FindGameObjectWithTag ("levelFinishPanel").GetComponentInChildren(typeof(RectTransform),true).gameObject;
		panel.SetActive (true);
		panel.GetComponent<levelFinishScreen> ().nextLevel = nextLevel;
	}

	void HandleEventonLevelSwitch (string nextLevel)
	{	
		Time.timeScale = 1.0f;
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
		GameObject newPackage;

		if (Gamemanager.Instance.CheckpointReached != 0) {
			GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
			foreach(GameObject checkpoint in checkpoints){
				if(checkpoint.GetComponent<CheckpointController>().checkpointNum == Gamemanager.Instance.CheckpointReached){
					transform = checkpoint.GetComponent<CheckpointController>().playerSpawn;
				}
			}
		}

		if (Gamevariables.magicPackage) {
			newPackage = (GameObject)Instantiate (PickUpMagicPrefab, transform.position, transform.rotation);
		} else {
			newPackage = (GameObject)Instantiate (PickUp1Prefab, transform.position, transform.rotation);
		}
		NetworkServer.Spawn (newPackage);
	}

	[Server]
	void HandleEventonPlayerDeath (GameObject player)
	{	
		NetworkConnection conn = player.GetComponent<NetworkIdentity> ().connectionToClient;
		short playerControllerId = player.GetComponent<NetworkIdentity> ().playerControllerId;
		Transform transform = GameObject.FindWithTag ("SpawnLocation").transform;
		GameObject package = GameObject.FindWithTag ("Package1");
		if (Gamemanager.Instance.CheckpointReached != 0) {
			GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
			foreach(GameObject checkpoint in checkpoints){
				if(checkpoint.GetComponent<CheckpointController>().checkpointNum == Gamemanager.Instance.CheckpointReached){
					transform = checkpoint.GetComponent<CheckpointController>().playerSpawn;
				}
			}
		}
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
		Gamemanager.Instance.CheckpointReached = 0;
		/*
		Analytics.CustomEvent ("Level Started", new Dictionary<string, object> {
			{ "Levelname", nextLevel }
		});
		*/
		networkmanager.ServerChangeScene (nextLevel);
		Gamevariables.currentLevel = NetworkManager.networkSceneName;
	}


	void SendClientReadyMsg(){
		var msg = new IntegerMessage(1);
		m_client.Send (ClientReadyMsg, msg);
	}

	void onClientReadyMsg(NetworkMessage netMsg){
		clientEndLevelReady = true;
	}
}
