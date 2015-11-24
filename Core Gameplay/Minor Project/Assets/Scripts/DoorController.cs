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
			door.transform.position = new Vector3 (-9.0f, 16.0f, 0.0f);
			doorOpen = true;
			RpcDoorOpen();
		} else {
			door.transform.position = new Vector3 (-9.0f, 4.1f, 0.0f);
			doorOpen = false;
			RpcDoorClose();
		}
	}

	[ClientRpc]
	void RpcDoorOpen() {
		door.transform.position = new Vector3 (-9.0f, 16.0f, 0.0f);
	}

	[Command]
	void CmdDoorOpen() {
		door.transform.position = new Vector3 (-9.0f, 16.0f, 0.0f);
	}

	[ClientRpc]
	void RpcDoorClose() {
		door.transform.position = new Vector3 (-9.0f, 4.1f, 0.0f);
	}

	[Command]
	void CmdDoorClose() {
		door.transform.position = new Vector3 (-9.0f, 4.1f, 0.0f);
	}
}
