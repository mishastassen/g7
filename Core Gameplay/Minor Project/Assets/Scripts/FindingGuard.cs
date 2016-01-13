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

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		agent = GetComponentInParent<NavMeshAgent> ();
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
		
		Vector3 playerPos = player.transform.position;
		agent.destination = playerPos;

		bool shouldStrike = IsInStrikingDistance (playerPos);
		Strike (shouldStrike);
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
