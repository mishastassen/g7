using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PickUpMagicController : NetworkBehaviour {
	
	public GameObject PickUpMagicSpawnPrefab;
	
	private bool isDestroyed = false;
	// Use this for initialization
	void Start () {
		if (isServer) {
			if(GameObject.FindWithTag("PickUpMagicSpawn") == null){
				GameObject newSpawn = (GameObject)Instantiate (PickUpMagicSpawnPrefab, this.transform.position, this.transform.rotation);
			}
		}
	}
	
	[ServerCallback]
	void OnTriggerEnter(Collider other)
	{	
		if (other.tag == "DeliveryZone") {
			string nextlevel = other.GetComponent<DeliveryZoneController>().nextLevel;
			Eventmanager.Instance.triggerLevelFinished(nextlevel);
		}
		
		if ( other.tag == "DeathZone" && !isDestroyed) {
			isDestroyed = true;
			Eventmanager.Instance.triggerPackageDestroyed();
		}
	}
	
}