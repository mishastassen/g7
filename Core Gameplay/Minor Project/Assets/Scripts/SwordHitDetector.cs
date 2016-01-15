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

	// if script is attached to guard
	void OnTriggerEnter (Collider other) {
		if (isServer && other.tag == "Sword") {
			Gamevariables.guardsDeathCount++;
			Destroy(this.gameObject);

		}
	}

	/*
	// If script is attached to player 
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Guard") {
			//Debug.Log ("Destroy this gameobject");
			// FIXME: should be made multiplayer proof
			//Destroy (this.transform.parent.gameObject);

			NetworkIdentity nid = other.gameObject.GetComponent<NetworkIdentity>();
			//nid.netId;
			//NetworkInstanceId g = nid.;
			Debug.Log ("Guard geraakt, networkid: "+nid);
		}
	}
	*/

}
