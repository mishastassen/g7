using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class SwitchController : NetworkBehaviour {

	public int switchID;
	bool eventEnabled;
	private Animator anim;
	private bool isSwitched;
	private bool playedSound;
	private AudioSource switchSound;
	//private BoxCollider collider;

	int ExtractIDFromName(String name) {
		int from = name.IndexOf ('(')+1;
		int to = name.IndexOf (')');
		int res=-1;
		if (from<to && int.TryParse(name.Substring (from,to-from), out res))
			return res;
		return res;
	}
		
	void Start () {
		switchID = ExtractIDFromName (this.name);
		Eventmanager.Instance.EventonSwitchPulled += switchPulled;
		eventEnabled = true;
		anim = this.GetComponent<Animator> ();
		isSwitched = false;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
		playedSound = false;
		switchSound = GetComponent<AudioSource> ();
		//collider = GetComponent<BoxCollider> ();
	}

	void OnDisable(){
		if (eventEnabled) {
			Eventmanager.Instance.EventonSwitchPulled -= switchPulled;
			eventEnabled = false;
		}
	}
		
	void switchPulled(int id){
		if (id != switchID) {
			return;
		}

		if (!isSwitched) {
			isSwitched = true;
			anim.SetBool ("isPressed", true);
			if (!playedSound) {
				switchSound.Play ();
				playedSound = true;
			}
			//collider.enabled = false;
			//collider.isTrigger = false;
		} else {
			isSwitched = false;
		}
	}
}
	


