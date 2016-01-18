using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WakeGuardScript : NetworkBehaviour {

	public GameObject prefabFindingGuard;
	private Transform spawnLocation;

	private static float coolOffTime = 5.0f;
	private float lastSpawnTime = -coolOffTime;

	private bool GuardSpawned = false;

	// Use this for initialization
	void Start () {
		spawnLocation = transform.Find ("GuardSpawnLocation"); // GetComponentInChildren<Transform> ();
		// Debug.Log ("spawnLocation pos: "+spawnLocation.position);
	}

	void OnTriggerEnter (Collider other) {
		if (isServer && other.tag == "Player" && !GuardSpawned) {
			Debug.Log ("player entered.");
			GameObject findingGuard = (GameObject)Instantiate (prefabFindingGuard, spawnLocation.position, spawnLocation.rotation);
			NetworkServer.Spawn (findingGuard);
			GuardSpawned = true;
		}
	}
}
