using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class DoorController : NetworkBehaviour {

	private Rigidbody door;
	private Animator anim;
	private BoxCollider box;
	private float openedCenterY = 3.2f;
	private float openedSizeY = 0.4f;
	private float closedCenterY = 1.75f;
	private float closedSizeY = 3.5f;

	public int doorID;

	private bool doorOpen;
	private bool eventEnabled;

	int ExtractIDFromName(String name) {
		int from = name.IndexOf ('(')+1;
		int to = name.IndexOf (')');
		int res=-1;
		if (from<to && int.TryParse(name.Substring (from,to-from), out res))
			return res;
		return res;
	}

	void Start () {
		door = GetComponent<Rigidbody>();
		anim = GetComponent<Animator> ();
		box = GetComponent<BoxCollider> ();
		doorID = ExtractIDFromName (this.name);
		doorOpen = false;
		Eventmanager.Instance.EventonSwitchPulled += switchPulled;
		eventEnabled = true;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
	}
	
	void switchPulled(int id) {
		if (id != doorID)
			return;
		if (!doorOpen) {
			doorOpen = true;
			anim.SetBool ("isOpen", true);
			Vector3 center = box.center;
			center.y=openedCenterY;
			box.center = center;
			Vector3 size = box.size;
			size.y=openedSizeY;
			box.size = size;
		} else {
			doorOpen = false;
			anim.SetBool ("isOpen", false);
			Vector3 center = box.center;
			center.y=closedCenterY;
			box.center = center;
			Vector3 size = box.size;
			size.y=closedSizeY;
			box.size = size;
		}

	
	}
	
	void OnDisable(){
		if (eventEnabled) {
			Eventmanager.Instance.EventonSwitchPulled -= switchPulled;
			eventEnabled = false;
		}
	}
	
}
