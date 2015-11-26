using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DoorController : NetworkBehaviour {

	private Rigidbody door;

	[SyncVar]
	private bool doorOpen;
	

	void Start () {
		door = GetComponent<Rigidbody>();
		doorOpen = false;
		Eventmanager.Instance.EventonSwitchPulled += switchPulled;
	}
	
	void switchPulled() {
		if (!doorOpen) {
			doorOpen = true;
			door.transform.position = new Vector3 (-9.0f, 16.0f, 0.0f);
		} else {
			doorOpen = false;
			door.transform.position = new Vector3 (-9.0f, 4.1f, 0.0f);
		}
	}
	
}
