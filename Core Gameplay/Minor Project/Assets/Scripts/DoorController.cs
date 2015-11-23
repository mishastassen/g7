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
	}

	void OnEnable() {
		Eventmanager.onSwitchPulled += switchPulled;
	}

	void OnDisable() {
		Eventmanager.onSwitchPulled -= switchPulled;
	}
	
	void switchPulled() {
		if (!doorOpen) {
			door.transform.position = new Vector3(11.0f, 16.0f, 0.0f);
			doorOpen = true;
			CmdOpenDoor();
		} else {
			door.transform.position = new Vector3(11.0f, 4.1f, 0.0f);
			doorOpen = false;
			CmdCloseDoor();
		}
	}

	[Command]
	void CmdOpenDoor() {
		door.transform.position = new Vector3(11.0f, 16.0f, 0.0f);
	}

	[Command]
	void CmdCloseDoor() {
		door.transform.position = new Vector3(11.0f, 4.1f, 0.0f);
	}

}
