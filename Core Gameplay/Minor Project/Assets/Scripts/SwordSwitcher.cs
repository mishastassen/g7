using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SwordSwitcher : NetworkBehaviour {

	public GameObject SwoldOnHip;
	public GameObject SwoldInHand;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGrab() {
		Debug.Log ("Sword grabbed: "+SwoldOnHip.name);
		SwoldOnHip.SetActive (false);
		SwoldInHand.SetActive (true);
	}

	public void OnInPocket() {
		Debug.Log ("Sword in pocket.");
		SwoldOnHip.SetActive (true);
		SwoldInHand.SetActive (false);
	}
}
