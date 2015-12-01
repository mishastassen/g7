using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DoorController : NetworkBehaviour {

	private Rigidbody door;
	private Animator anim;

	[SyncVar]
	private bool doorOpen;
	

	void Start () {
		door = GetComponent<Rigidbody>();
		anim = GetComponent<Animator> ();
		doorOpen = false;
		Eventmanager.Instance.EventonSwitchPulled += switchPulled;
	}
	
	void switchPulled() {
		if (!doorOpen) {
			doorOpen = true;
			anim.SetBool ("isOpen", true);
		} else {
			doorOpen = false;
			anim.SetBool ("isOpen", false);
		}
	}
	
}
