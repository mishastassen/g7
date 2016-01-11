using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;


public class SwordHitDetector : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if (isServer && other.tag == "Sword") 
			//Debug.Log ("Destroy this gameobject");
			// FIXME: should be made multiplayer proof
			Destroy (this.transform.parent.gameObject);

	}
}
