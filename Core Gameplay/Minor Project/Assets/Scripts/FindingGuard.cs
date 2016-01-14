using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class FindingGuard : NetworkBehaviour {

	private Animator anim;

	NavMeshAgent agent;

	private List <Collider> TriggerList= new List<Collider>();

	private GameObject[] players;
	private GameObject targetPlayer;
	private Vector3 playerPos;

	private float strikingDistance;
	private bool waiting;

	// initial cooldown time
	private static float startCoolDownTime = 0.8f;
	// minimum time between consecutive strikes
	private float coolDownTime = startCoolDownTime;
	private float lastStrike = -1f;

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
			//Debug.Log ("finding guard is niet op server");
			return;
		}
		TriggerList.RemoveAll(x => x == null);

		FindPlayers ();
		targetPlayer = GetClosestPlayer ();
		// Sometimes there are no players in the scene (for a short moment)
		if (targetPlayer == null)
			return;

		UpdateStrength ();

		bool shouldStrike = IsInStrikingDistance (playerPos) && Time.time > lastStrike + coolDownTime;
		if(shouldStrike)
			lastStrike = Time.time;
		Strike (shouldStrike);

		Debug.Log ("De waarde van de shouldstrike bool = " + shouldStrike);

		if (shouldStrike) {
			agent.enabled = false;
		} else {
			agent.enabled = true;
			playerPos = targetPlayer.transform.position;
			agent.destination = playerPos;
		}
	}

	void FindPlayers() {
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	GameObject GetClosestPlayer() {
		GameObject closestPlayer = null;
		float shortestDistance = 1e9f;
		foreach (GameObject p in players)
			if (Vector3.Distance (p.transform.position, this.transform.position) < shortestDistance) {
				shortestDistance = Vector3.Distance (p.transform.position, this.transform.position);
				closestPlayer = p;
			}
		return closestPlayer;
	}

	// Strength should be between 0 (really weak) and 1 (strong)
	void UpdateStrength() {
		float strength = BaseGuard.getStrength ();
		Debug.Log("Strength: "+strength);
		coolDownTime = startCoolDownTime + 3*(1f - strength);
		anim.speed = 0.2f + 0.8f*strength;
	}

	bool IsInStrikingDistance(Vector3 playerPos) {
		Vector3 curPos = this.transform.position;
		//targetPlayer = GameObject.FindGameObjectWithTag ("Player");
		playerPos = targetPlayer.transform.position;
		return  Vector3.Distance (curPos, playerPos) < strikingDistance;
	}

	void Strike(bool shouldStrike) {
		if (shouldStrike) {
			if (!waiting) {
				StartCoroutine (WaitBeforeHit ());
			}
		}
		else{
			if (anim.GetBool ("isStriking")) {
				RpcStopStrike ();
			}
		}
	}

	void PrintTriggerList() {
		Debug.Log ("triggerlist:");
		foreach (Collider c in TriggerList) {
			Debug.Log (c.tag);
		}
	}

	IEnumerator WaitBeforeHit(){
		waiting = true;
		Debug.Log ("Wait for striking");
		yield return new WaitForSeconds(1f);
		RpcStartStrike ();
		waiting = false;
	}

	[ClientRpc]
	void RpcStartStrike(){
		anim.SetBool ("isStriking", true);
	}

	[ClientRpc]
	void RpcStopStrike(){
		anim.SetBool ("isStriking", false);
	}

}
