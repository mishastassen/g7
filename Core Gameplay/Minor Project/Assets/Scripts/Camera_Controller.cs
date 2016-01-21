using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_Controller : MonoBehaviour {

	private List<GameObject> players;

	private Vector2 windowCenter;
	private Vector2 previousCenter;
	private Vector3 newLocation;
	private Vector3 oldLocation;

	float limitX;
	float limitY;

	private Camera cam;
	private float zoom;

	private string currentLevel;
	private bool ready = true;

	Vector3 playerPos;

	private float fovR;
	private float zoomDistance = 60f;

	// Use this for initialization
	void Start () {
		currentLevel = Gamevariables.currentLevel;
		players = new List<GameObject>();
		windowCenter.x = this.GetComponent<Transform> ().position.x;
		windowCenter.y = this.GetComponent<Transform> ().position.y;
		previousCenter = windowCenter;
		cam = this.GetComponent<Camera> ();
		cam.fieldOfView = 35f;
		fovR = Mathf.Deg2Rad * cam.fieldOfView;
		zoom = cam.fieldOfView;
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
		// als er twee spelers zijn
		if (players.Count == 2) {
			if (ready) {
				updateCameraLocation ();
			}
			// als er (nog maar) 1 speler is, dan volgt de camera altijd deze 
		} else if (players.Count == 1) {
			if (ready) {
				float x = players [0].GetComponent<Transform> ().position.x;
				float y = players [0].GetComponent<Transform> ().position.y;
				float z = -60f;
				newLocation.Set (x, y, z);
				this.GetComponent<Transform> ().position = newLocation;
			}
		// als er geen spelers zijn, focussen op het 0 punt
		} else if (players.Count == 0) {
			if (ready) {
				newLocation.Set (0f, 0f, -60f);
				this.GetComponent<Transform> ().position = newLocation;
			}
		// het laatste geval voor als er 3 of meer spelers zijn. 
		} else {
			if (ready) {
				updateCameraLocation ();
			}
		}
	}

	void addplayer(GameObject player){	//Add new player to list of player objects
		players.Add (player);
	}

	void removeplayer(GameObject player){
		players.Remove(player);
	}

	void updateCameraLocation(){
		float z = this.GetComponent<Transform> ().position.z;

		float x0 = players [0].GetComponent<Transform> ().position.x;
		float x1 = players [1].GetComponent<Transform> ().position.x;
		// reken het gemiddelde uit in de X richting
		float xC = (x0 + x1) / 2f;

		float y0 = players [0].GetComponent<Transform> ().position.y;
		float y1 = players [1].GetComponent<Transform> ().position.y;
		// reken het gemiddelde uit in de Y richting
		float yC = (y0 + y1) / 2f;

		// afstanden tussen de spelers in X en Y
		float distanceX = Mathf.Abs (x0 - x1);
		float distanceY = Mathf.Abs (y0 - y1);

		float zC;

		// de regels hieronder bepalen wat leidend is: de X richting of de Y richting
		if (distanceY > distanceX * (9f / 16f)) {
			zC = 1.4f * (distanceY / (Mathf.Tan (fovR))) / 1.0f;
			Debug.Log ("should zoom out due to Y now");
		} else{
			zC = (distanceX / (Mathf.Tan (fovR))) / 1.4f;
		}
			
		if (zC < 60f) {
			zC = 60f;
		}

		// geef de camera de goede positie mee
		newLocation.Set (xC, yC, -zC);
		this.GetComponent<Transform> ().position = newLocation;
	}
}