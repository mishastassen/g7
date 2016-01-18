using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityAnalyticsHeatmap;

public class PickUp1Controller : NetworkBehaviour {

	public GameObject PickUp1SpawnPrefab;
	public bool magicPackage;

	private bool isDestroyed = false;
	// Use this for initialization
	void Start () {
		Gamevariables.magicPackage = magicPackage;
		if (isServer) {
			if(GameObject.FindWithTag("PickUp1Spawn") == null){
				GameObject newSpawn = (GameObject)Instantiate (PickUp1SpawnPrefab, this.transform.parent.transform.position, this.transform.parent.transform.rotation);
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
			Analytics.CustomEvent ("Package Destroyed", new Dictionary<string, object> {
				{ "Levelname", Gamevariables.currentLevel }
			});
			HeatmapEvent.Send ("Package Destroyed Heatmap", this.transform.position, new Dictionary<string, object> {
				{ "Levelname", Gamevariables.currentLevel }
			});
			Eventmanager.Instance.triggerPackageDestroyed();
		}
	}

}
