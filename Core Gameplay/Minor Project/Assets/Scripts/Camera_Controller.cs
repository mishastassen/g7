using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera_Controller : MonoBehaviour {

	private List<GameObject> players;

	public float minzoom;
	public float maxzoom;
	public float windowx;
	public float windowy;

	private Vector2 windowCenter;
	private Vector2 previousCenter;
	private Vector3 newLocation;

	float limitX;
	float limitY;

	private Camera cam;
	private float zoom;
	private float zoomSensitivity = 15f;
	private float zoomSpeed = 2f;
	private float zoomMin = 25f;
	private float zoomMax = 45f;

	// Use this for initialization
	void Start () {
		players = new List<GameObject>();
		windowCenter.x = this.GetComponent<Transform> ().position.x;
		windowCenter.y = this.GetComponent<Transform> ().position.y;
		previousCenter = windowCenter;
		limitX = 30f;
		limitY = 20f;
		cam = this.GetComponent<Camera> ();
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
		if (players.Count > 0) {
			updateCameraLocation ();
		}
		// Debug.Log ("The current field of view is " + cam.fieldOfView);
		Debug.Log(players.Count);
	}

	void addplayer(GameObject player){	//Add new player to list of player objects
		players.Add (player);
	}

	void removeplayer(GameObject player){
		players.Remove(player);
	}

	void updateCameraLocation(){
		Vector3 center = Vector3.zero;
		foreach (GameObject player in players) {
			if (player == null) {
				removeplayer (player);
				return;
			} else {
				center += player.GetComponent<Transform> ().position;
			}
		}
		center /= (float)players.Count;
		//newLocation.z = -40.0f;
		if (Mathf.Abs (center.x - windowCenter.x) > windowx) {
			windowCenter.x += center.x - previousCenter.x;
		}
		if (Mathf.Abs (center.y - windowCenter.y) > windowy) {
			windowCenter.y += center.y + -previousCenter.y;
		}
		newLocation.Set(windowCenter.x, windowCenter.y + 5.0f, -60.0f);
		previousCenter = new Vector2 (center.x, center.y);

		foreach (GameObject player in players) {
			if (Mathf.Abs (player.GetComponent<Transform> ().position.x - newLocation.x) > Mathf.Abs(limitX)) {
				cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, cam.fieldOfView*1.05f, Time.deltaTime * zoomSpeed);
				limitX *= (player.GetComponent<Transform> ().position.x / limitX);
			} else {
				limitX = 30f;
				cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,35,Time.deltaTime*(zoomSpeed/2f));
			}
		}
		this.GetComponent<Transform> ().position = newLocation;
	}
}