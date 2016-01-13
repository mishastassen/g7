using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class FindingGuard : NetworkBehaviour {

	private Animator anim;

	private GameObject player;
	NavMeshAgent agent;

	private List <Collider> TriggerList= new List<Collider>();
	private Vector3 playerPos;
	private float strikingDistance;
	private bool waiting;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		agent = GetComponentInParent<NavMeshAgent> ();
		if (!isServer) {
			agent.enabled = false;
		}
		strikingDistance = 12f;
		waiting = false;
	}

	void Update () {
		if (!isServer) {
			Debug.Log ("finding guard is niet op server");
			return;
		}
		TriggerList.RemoveAll(x => x == null);

		if(player==null)
			player = GameObject.FindGameObjectWithTag ("Player");
		// Sometimes there are no players in the scene (for a short moment)
		if (player == null)
			return;

		bool shouldStrike = IsInStrikingDistance (playerPos);
		Strike (shouldStrike);

		Debug.Log ("De waarde van de shouldstrike bool = " + shouldStrike);

		if (shouldStrike) {
			agent.enabled = false;
		} else {
			agent.enabled = true;
			playerPos = player.transform.position;
			agent.destination = playerPos;
		}
	}

	bool IsInStrikingDistance(Vector3 playerPos) {
		Vector3 curPos = this.transform.position;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerPos = player.transform.position;
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
