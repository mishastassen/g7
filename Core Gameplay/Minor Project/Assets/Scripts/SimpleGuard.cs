using UnityEngine;
using System.Collections;

public class SimpleGuard : MonoBehaviour {

	public GameObject enemy;
	private GameObject player;
	NavMeshAgent agent;
	private Transform PlayerPos;

	// Use this for initialization
	void Start () {
		agent = GetComponentInParent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		player = GameObject.FindGameObjectWithTag ("Player");
		PlayerPos = player.transform;
		agent.destination = PlayerPos.position;
	}
}
