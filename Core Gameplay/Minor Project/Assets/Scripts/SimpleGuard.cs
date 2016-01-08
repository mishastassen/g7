using UnityEngine;
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
		foreach (Collider c in TriggerList) {
			if (c.tag == "Player") {
				//Debug.Log ("Player found!");
				player = c.gameObject;
				//player = GameObject.FindGameObjectWithTag ("Player");
				Vector3 playerPos = player.transform.position;
				agent.destination = playerPos;

				if (isServer && IsInStrikingDistance (playerPos))
					Strike ();
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
		}

		if (other.tag == "Player") {
			Debug.Log ("Player entered!");
		} 
	}

	void OnTriggerExit (Collider other){
		//if the object is in the list of triggers, remove it
		if (TriggerList.Contains (other)) {
			if (other.tag == "Player") {
				Debug.Log ("Player removed!");
			}
			TriggerList.Remove (other);
		}
	}

	bool IsInStrikingDistance(Vector3 playerPos) {
		Vector3 curPos = this.transform.position;
		return  Vector3.Distance (curPos, playerPos) < 5;
	}

	void Strike() {
		anim.SetBool ("isStriking", true);
	}
}
