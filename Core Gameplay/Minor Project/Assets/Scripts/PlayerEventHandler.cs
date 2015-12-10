using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerEventHandler : MonoBehaviour {

	private PlayerController pc;
	private bool enabled;

	// Use this for initialization
	void OnEnable () {
		Eventmanager.Instance.EventonPackagePickup += HandleEventonPackagePickup;
		Eventmanager.Instance.EventonPackageDrop += HandleEventonPackageDrop;
		Eventmanager.Instance.EventonPackageThrow += HandleEventonPackageThrow;
		pc = this.GetComponent<PlayerController> ();
		enabled = true;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
	}

	void OnDisable(){
		if (enabled) {
			Eventmanager.Instance.EventonPackagePickup -= HandleEventonPackagePickup;
			Eventmanager.Instance.EventonPackageDrop -= HandleEventonPackageDrop;
			Eventmanager.Instance.EventonPackageThrow -= HandleEventonPackageThrow;
			enabled = false;
		}
	}

	void HandleEventonPackagePickup(NetworkInstanceId netID, string tag){
		if (netID == this.gameObject.GetComponent<NetworkIdentity> ().netId) {
			if (tag == "PickUp1") {
				GameObject other = GameObject.FindWithTag(tag);
				other.transform.parent.SetParent(gameObject.GetComponent<Rigidbody>().transform);
				other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
				other.transform.parent.localPosition = new Vector3(2,3,0);
				this.GetComponent<PlayerController>().carriedPackage = other.transform.parent;
				this.GetComponent<PlayerController>().hasPackage = true;
			} else if (tag == "PickUpMagic") {
				GameObject other = GameObject.FindWithTag(tag);
				other.transform.parent.SetParent(gameObject.GetComponent<Rigidbody>().transform);
				other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
				other.transform.parent.localPosition = new Vector3(2,3,0);
				this.GetComponent<PlayerController>().carriedPackage = other.transform.parent;
				this.GetComponent<PlayerController>().hasPackage = true;
				this.GetComponent<PlayerController>().hasMagicPackage = true;
			}
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
			pc.hasMagicPackage = false;
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
			this.GetComponent<PlayerController>().hasMagicPackage = false;
		}
	}

}
