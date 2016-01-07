using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class FindingGuard : MonoBehaviour {

	private GameObject player;
	NavMeshAgent agent;
	private Transform PlayerPos;

	private List <Collider> TriggerList= new List<Collider>();

	// Use this for initialization
	void Start () {
		agent = GetComponentInParent<NavMeshAgent> ();
	}

	void Update () {
		player = GameObject.FindGameObjectWithTag ("Player");
		PlayerPos = player.transform;
		agent.destination = PlayerPos.position;
	}

	void OnTriggerEnter (Collider other){
		if (!TriggerList.Contains (other)) {
			//add the object to the list
			TriggerList.Add (other);
		}
	}

	void OnTriggerExit (Collider other){
		//if the object is in the list of triggers, remove it
		if (TriggerList.Contains (other)) {
			if (other.tag == "Player") {
			}
			TriggerList.Remove (other);
		}
	}
}
