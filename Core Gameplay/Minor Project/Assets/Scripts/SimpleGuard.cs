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
	private Vector3 playerPos;
	private float strikingDistance;
	private bool waiting;

	// Use this for initialization
	void Start () {
		agent = GetComponentInParent<NavMeshAgent> ();
		ResetLoc = this.transform.position;
		anim = GetComponentInChildren<Animator> ();
		strikingDistance = 12f;
		waiting = false;
	}

	void Update () {
		if (!isServer) {
			Debug.Log ("Simple guard is niet op server");
			return;
		}

		if(player==null)
			player = GameObject.FindGameObjectWithTag ("Player");
		// Sometimes there are no players in the scene (for a short moment)
		if (player == null) {
			Debug.Log ("Er zijn geen players");
			return;
		}

		TriggerList.RemoveAll(x => x == null);
		foreach (Collider c in TriggerList) {
			if (c.tag == "Player") {
				player = c.gameObject;
				Vector3 playerPos = player.transform.position;

				bool shouldStrike = IsInStrikingDistance (playerPos);
				Strike (shouldStrike);

				// Debug.Log ("De waarde van de shouldstrike bool = " + shouldStrike);

				if (shouldStrike) {
					agent.enabled = false;
				} else {
					agent.enabled = true;
					playerPos = player.transform.position;
					anim.speed = 1;
					agent.destination = playerPos;
				}
				return;
			} 
		}
		agent.enabled = true;
		stopWalking ();
		agent.destination = ResetLoc;
	}

	void stopWalking (){
		Vector3 curPos = this.transform.position;
		Debug.Log ("De guard is zo dicht van zijn reset locatie: "+Vector3.Distance (curPos, ResetLoc));
		if (Vector3.Distance (curPos, ResetLoc) < 1f) {
			anim.speed = 0;
		} else {
			anim.speed = 1;
		}
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
		player = GameObject.FindGameObjectWithTag ("Player");
		playerPos = player.transform.position;
		Debug.Log (Vector3.Distance(curPos,playerPos));
		return  Vector3.Distance (curPos, playerPos) < strikingDistance;
	}

	void Strike(bool shouldStrike) {
		if (shouldStrike) {
			if (!waiting) {
				StartCoroutine (waitBeforeHit ());
			}
		}
		else{
			anim.SetBool ("isStriking", shouldStrike);
		}
	}

	void PrintTriggerList() {
		Debug.Log ("triggerlist:");
		foreach (Collider c in TriggerList) {
			Debug.Log (c.tag);
		}
	}

	IEnumerator waitBeforeHit(){
		waiting = true;
		Debug.Log ("Wait for striking");
		yield return new WaitForSeconds(1f);
		anim.SetBool ("isStriking", true);
		waiting = false;
	}
}
