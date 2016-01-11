﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class SimpleGuard : NetworkBehaviour {

	private Animator anim;

	private GameObject player;
	NavMeshAgent agent;
	private Vector3 ResetLoc;

	private List <Collider> TriggerList= new List<Collider>();

	// Use this for initialization
	void Start () {
		agent = GetComponentInParent<NavMeshAgent> ();
		ResetLoc = this.transform.position;

		anim = GetComponentInChildren<Animator> ();
	}

	void Update () {
		// If an object in triggerlist gets destroyed, OnTriggerExit isn't called, but object should be removed
		TriggerList.RemoveAll(x => x == null);
		foreach (Collider c in TriggerList) {
			if (c.tag == "Player") {
				player = c.gameObject;
				Vector3 playerPos = player.transform.position;
				agent.destination = playerPos;

				if (isServer) {
					bool shouldStrike = IsInStrikingDistance (playerPos);
					Strike (shouldStrike);
				}
				return;
			} 
		}
		//Debug.Log ("Should start walking back now!");
		agent.destination = ResetLoc;
	}

	void OnTriggerEnter (Collider other){
		if (!TriggerList.Contains (other)) {
			//add the object to the list
			TriggerList.Add (other);
			Debug.Log ("triggerenter: "+other.tag);
		}

		if (other.tag == "Player") {
			//Debug.Log ("Player entered!");
		} 
		//PrintTriggerList();
	}

	void OnTriggerExit (Collider other){
		//if the object is in the list of triggers, remove it
		if (TriggerList.Contains (other)) {
			Debug.Log ("triggerexit: "+other.tag);
			if (other.tag == "Player") {
				//Debug.Log ("Player removed!");
			}
			TriggerList.Remove (other);
		}
		//PrintTriggerList ();
	}

	bool IsInStrikingDistance(Vector3 playerPos) {
		Vector3 curPos = this.transform.position;
		return  Vector3.Distance (curPos, playerPos) < 8;
	}

	void Strike(bool shouldStrike) {
		anim.SetBool ("isStriking", shouldStrike);
	}

	void PrintTriggerList() {
		Debug.Log ("triggerlist:");
		foreach (Collider c in TriggerList) {
			Debug.Log (c.tag);
		}
	}


}
