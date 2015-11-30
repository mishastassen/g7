using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerEventHandler : MonoBehaviour {

	private PlayerController pc;

	// Use this for initialization
	void Start () {
		Eventmanager.Instance.EventonPackagePickup += HandleEventonPackagePickup;
		Eventmanager.Instance.EventonPackageDrop += HandleEventonPackageDrop;
		Eventmanager.Instance.EventonPackageThrow += HandleEventonPackageThrow;
		pc = this.GetComponent<PlayerController> ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void HandleEventonPackagePickup(NetworkInstanceId netID, string tag){
		if (netID == this.gameObject.GetComponent<NetworkIdentity> ().netId) {
			GameObject other = GameObject.FindWithTag(tag);
			other.transform.parent.SetParent(gameObject.GetComponent<Rigidbody>().transform);
			other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
			other.transform.parent.localPosition = new Vector3(0,3,2);
			this.GetComponent<PlayerController>().carriedPackage = other.transform.parent;
			this.GetComponent<PlayerController>().hasPackage = true;
		}
	}

	void HandleEventonPackageDrop(NetworkInstanceId netID){
		if (netID == this.gameObject.GetComponent<NetworkIdentity> ().netId) {
			//Remove package from collider list
			if(pc.TriggerList.Contains(pc.carriedPackage.gameObject.GetComponentsInChildren<Collider>()[1]))
			{
				//remove it from the list
				pc.TriggerList.Remove(pc.carriedPackage.gameObject.GetComponentsInChildren<Collider>()[1]);
			}

			//Drop package
			pc.carriedPackage.GetComponent<Rigidbody>().isKinematic = false;
			pc.carriedPackage.parent = null;
			pc.carriedPackage = null;
			pc.hasPackage = false;
		}
	}

	void HandleEventonPackageThrow (NetworkInstanceId netId)
	{
		if (netId == this.gameObject.GetComponent<NetworkIdentity> ().netId) {
			//Remove package from collider list
			if(pc.TriggerList.Contains(pc.carriedPackage.gameObject.GetComponentsInChildren<Collider>()[1]))
			{
				//remove it from the list
				pc.TriggerList.Remove(pc.carriedPackage.gameObject.GetComponentsInChildren<Collider>()[1]);
			}

			//Throw package
			Transform carriedPackage = pc.carriedPackage;
			carriedPackage.GetComponent<Rigidbody>().isKinematic = false;
			carriedPackage.GetComponent<Rigidbody> ().AddForce (new Vector3 (pc.facingRight * 750, 750, 0));
			carriedPackage.parent = null;
			carriedPackage = null;
			this.GetComponent<PlayerController>().hasPackage = false;
		}
	}
}
