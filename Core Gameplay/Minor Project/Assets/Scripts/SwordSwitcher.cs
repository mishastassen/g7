using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SwordSwitcher : NetworkBehaviour {

	public GameObject SwordOnHip;
	public GameObject SwordInHand;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGrab() {
		Debug.Log ("Sword grabbed: "+SwordOnHip.name);
		SwordOnHip.SetActive (false);
		SwordInHand.SetActive (true);
	}

	public void OnInPocket() {
		Debug.Log ("Sword in pocket.");
		SwordOnHip.SetActive (true);
		SwordInHand.SetActive (false);
	}
}
