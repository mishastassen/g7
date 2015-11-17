using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_Controller : MonoBehaviour {

	private List<GameObject> players;

	public float minzoom;
	public float maxzoom;

	// Use this for initialization
	void Start () {
		players = new List<GameObject>();
	}

	void OnEnable(){
		Eventmanager.onPlayerAdded += addplayer;	//Start listening to onPlayerAdded event when camera is enabled
		Eventmanager.onPlayerRemoved += removeplayer;
	}

	void OnDisable(){
		Eventmanager.onPlayerAdded -= addplayer;	//Stop listening to onPlayerAdded event when camera is disabled
		Eventmanager.onPlayerRemoved -= removeplayer;
	}

	void Update(){
		updateCameraLocation ();
	}

	void addplayer(GameObject player){	//Add new player to list of player objects
		players.Add (player);
	}

	void removeplayer(GameObject player){
		players.Remove(player);
	}

	void updateCameraLocation(){
		Vector3 newLocation = Vector3.zero;
		foreach (GameObject player in players) {
			newLocation += player.GetComponent<Transform>().position;
		}
		newLocation /= (float)players.Count;
		newLocation.z = -40.0f;
		this.GetComponent<Transform> ().position = newLocation;
		Debug.Log (newLocation);
	}
}