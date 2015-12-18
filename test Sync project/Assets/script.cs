using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class script : NetworkBehaviour {

	private EventScript eventscript;
	private NumberScript numberscript;

	void Start(){
		eventscript = (EventScript) GameObject.FindObjectOfType (typeof(EventScript));
		numberscript = (NumberScript) GameObject.FindObjectOfType (typeof(NumberScript));
		if (isLocalPlayer) {
			numberscript.startListener ();
		};
	}

	void Update () {
		if (isLocalPlayer) {
			if (Input.GetKeyDown ("space")) {
				CmdSpacePressed ();
				Debug.Log ("Space pressed");
			}
			if (Input.GetKeyDown ("return")) {
				if (isServer) {
					RpcEnterPressed ();
				} else {
					numberscript.count ();
					CmdEnterPressed ();
				}
				Debug.Log ("Enter pressed");
			}
		}
	}

	[Command]
	void CmdSpacePressed(){
		Debug.Log ("Space command");
		eventscript = (EventScript) GameObject.FindObjectOfType (typeof(EventScript));
		eventscript.GetComponent<EventScript> ().triggerSpaceEvent ();
	}

	[Command]
	void CmdEnterPressed(){
		Debug.Log ("Enter Cmd");
		numberscript = (NumberScript) GameObject.FindObjectOfType (typeof(NumberScript));
		numberscript.count ();
	}

	[ClientRpc]
	void RpcEnterPressed(){
		Debug.Log ("Enter Rpc");
		numberscript = (NumberScript) GameObject.FindObjectOfType (typeof(NumberScript));
		numberscript.count ();
	}

}
