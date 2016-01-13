using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WakeGuardScript : NetworkBehaviour {

	public Transform findingGuard;
	private Transform spawnLocation;

	public GameObject inactiveFindingGuard;

	private static float coolOffTime = 5.0f;
	private float lastSpawnTime = -coolOffTime;

	// Use this for initialization
	void Start () {
		spawnLocation = transform.Find ("GuardSpawnLocation"); // GetComponentInChildren<Transform> ();
		Debug.Log ("spawnLocation pos: "+spawnLocation.position);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter (Collider other) {
		//*
		if (inactiveFindingGuard != null) {
			if (isServer && other.tag == "Player" && !inactiveFindingGuard.activeSelf) {
				Debug.Log ("player entered.");
				//inactiveFindingGuard.SetActive (true);
				RpcSetActiveGuard ();
				NetworkServer.Spawn (inactiveFindingGuard);
			}
		}
		/*/
		if (isServer && other.tag=="Player") {
			if (Time.time > lastSpawnTime + coolOffTime) {
				lastSpawnTime = Time.time;
				GameObject newGuard = (GameObject)Instantiate (findingGuard, spawnLocation.position, spawnLocation.rotation);
				NetworkServer.Spawn (newGuard);
			}
		} 
		//*/
	}

	[ClientRpc]
	void RpcSetActiveGuard(){
		inactiveFindingGuard.SetActive (true);
	}

}
