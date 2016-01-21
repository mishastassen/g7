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
	private Vector3 oldLocation;

	float limitX;
	float limitY;

	private Camera cam;
	private float zoom;
	private float zoomSensitivity = 15f;
	private float zoomSpeed = 0.5f;
	private float zoomMin = -25f;
	private float zoomMax = 42.5f;
	private float x;
	private float y;
	private float z;
	private bool zooming = false;

	private string currentLevel;
	private GameObject deliveryZone;
	private bool ready = true;
	private bool isStarted = false;

	private float lerpTime;
	private float currentLerpTime;
	Vector3 playerPos;

	// Use this for initialization
	void Start () {
		currentLevel = Gamevariables.currentLevel;
		players = new List<GameObject>();
		windowCenter.x = this.GetComponent<Transform> ().position.x;
		windowCenter.y = this.GetComponent<Transform> ().position.y;
		previousCenter = windowCenter;
		limitX = 22f;
		limitY = 10f;
		cam = this.GetComponent<Camera> ();
		zoom = cam.fieldOfView;
		playerPos = (this.GetComponent<Transform> ().position);
		newLocation = playerPos;
		// this.GetComponent<Transform>().position.Set(0f,0f,-60f);
		z = -60f;
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
			if (ready) {
				updateCameraLocation ();
			}
		}
		// Debug.Log ("The current field of view is " + cam.fieldOfView);
		// Debug.Log(players.Count);
	}

	void addplayer(GameObject player){	//Add new player to list of player objects
		players.Add (player);
	}

	void removeplayer(GameObject player){
		players.Remove(player);
	}

	void updateCameraLocation(){
		oldLocation = newLocation;
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

		x = windowCenter.x;
		y = windowCenter.y;
		previousCenter = new Vector2 (center.x, center.y);
		Debug.LogWarning (players.Count);
		foreach (GameObject player in players) {
			if (Mathf.Abs (player.GetComponent<Transform> ().position.x - oldLocation.x) > Mathf.Abs (limitX)) {
				float newFieldofView = updateFoV (cam.fieldOfView);
				if (Mathf.Abs (cam.fieldOfView - newFieldofView) > 0.3f){
					cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, newFieldofView, Time.deltaTime * zoomSpeed);
				} 
				//z = oldLocation.z * 1.005f;
				//limitX *= 1.00499f;
				z = oldLocation.z * 1.003f;
				limitX *= 1.00305f;
			} else if (Mathf.Abs (player.GetComponent<Transform> ().position.y - oldLocation.y) > Mathf.Abs (limitY)) {
				float newFieldofView = updateFoV (cam.fieldOfView);
				if (Mathf.Abs (cam.fieldOfView - newFieldofView) > 0.3f){
					cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, newFieldofView * 1.002f, Time.deltaTime * zoomSpeed);
				} 
				z = oldLocation.z * 1.005f;
				limitY *= 1.00505f;
			} else {
				updateLimitX (limitX);
				updateLimitY (limitY);
				z = updateZ (oldLocation.z);
				// cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, 35, Time.deltaTime * (zoomSpeed / 20f));
//				z = -60f;
//				Debug.LogWarning ("Reset limitX and limitY");
			}
		}

		newLocation.Set(x,y,z);
		Debug.LogWarning ("Zooming from: " + oldLocation + " to " + newLocation);
		this.GetComponent<Transform> ().position = Vector3.Lerp (oldLocation, newLocation, Time.deltaTime * zoomSpeed);
	}

	float updateFoV (float fov){
		if (fov * 1.02f > zoomMax) {
			return fov ;
		} else {
			return fov * 1.08f;
		}
	}

	float updateLimitX (float limitX){

		if ((limitX / 1.0005) < 30f) {
			return 30f;
		} else {
			return (limitX / 1.0005f);
		}
	}

	float updateLimitY (float limitY){

		if ((limitY / 1.0005f) < 15f) {
			return 15f;
		} else {
			return (limitY / 1.0005f);
		}
	}

	float updateZ (float z){
		if ((z / 1.005f) > -70f) {
			return -70f;
		} else {
			return (z / 1.005f);
		}
	}
}