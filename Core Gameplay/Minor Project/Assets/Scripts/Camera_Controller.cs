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

	private float fovR;
	private float zoomDistance = 60f;

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
		cam.fieldOfView = 35f;
		fovR = Mathf.Deg2Rad * cam.fieldOfView;
		zoom = cam.fieldOfView;
		playerPos = (this.GetComponent<Transform> ().position);
		newLocation = playerPos;
		this.GetComponent<Transform>().position.Set(playerPos.x,playerPos.y,-60f);
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
		float z = this.GetComponent<Transform> ().position.z;

		float x0 = players [0].GetComponent<Transform> ().position.x;
		float x1 = players [1].GetComponent<Transform> ().position.x;
		float xC = (x0 + x1) / 2f;

		float y0 = players [0].GetComponent<Transform> ().position.y;
		float y1 = players [1].GetComponent<Transform> ().position.y;
		float yC = (y0 + y1) / 2f;

		float distanceX = Mathf.Abs (x0 - x1);
		float distanceY = Mathf.Abs (y0 - y1);

		float zC;

		if (distanceY > distanceX * (9f / 16f)) {
			zC = 1.4f * (distanceY / (Mathf.Tan (fovR))) / 1.0f;
			Debug.Log ("should zoom out due to Y now");
		} else{
			zC = (distanceX / (Mathf.Tan (fovR))) / 1.4f;
		}
			
		if (zC < 60f) {
			zC = 60f;
		}

		Debug.Log (distanceY);

		newLocation.Set (xC, yC, -zC);

		this.GetComponent<Transform> ().position = newLocation;
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