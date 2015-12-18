using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EventScript : NetworkBehaviour {

	public delegate void spaceEvent();
	[SyncEvent]
	public event spaceEvent EventOnSpaceEvent;

	public void triggerSpaceEvent(){
		if (EventOnSpaceEvent != null) {
			Debug.Log ("Space event triggered");
			EventOnSpaceEvent ();
		}
	}
}
