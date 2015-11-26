using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DoorControllerServer : NetworkBehaviour {

	void OnEnable() {
		//Eventmanager.onSwitchPulled += CmdSwitchPulled;
	}
	
	void OnDisable() {
		//Eventmanager.onSwitchPulled -= CmdSwitchPulled;
	}

	[Command]
	void CmdSwitchPulled() {
		if (!isServer) {
			//Eventmanager.Instance.triggerSwitchPulled ();
		}
	}
}
