using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class SimpleGuard : MonoBehaviour {

	public GameObject enemy;
	private GameObject player;
	NavMeshAgent agent;
	private Transform PlayerPos;
	private Vector3 ResetLoc;

	private List <Collider> TriggerList= new List<Collider>();

	// Use this for initialization
	void Start () {
		agent = GetComponentInParent<NavMeshAgent> ();
		ResetLoc = enemy.transform.position;

	}
	
	// Update is called once per frame
//	void Update () {
//		player = GameObject.FindGameObjectWithTag ("Player");
//		PlayerPos = player.transform;
//		agent.destination = PlayerPos.position;
//	}

	void Update () {
		foreach (Collider c in TriggerList) {
			if (c.tag == "Player") {
				Debug.Log ("Player found!");
				player = GameObject.FindGameObjectWithTag ("Player");
				PlayerPos = player.transform;
				agent.destination = PlayerPos.position;
				return;
			} 
		}
		Debug.Log ("Should start walking back now!");
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
}
