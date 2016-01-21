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
	private float zoomMax = -70f;
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
		limitX = 30f;
		limitY = 20f;
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

		foreach (GameObject player in players) {
			Debug.LogWarning (Mathf.Abs (player.GetComponent<Transform> ().position.x - oldLocation.x) + " This should be greater than limitX: " + limitX);
			Debug.LogWarning (Mathf.Abs (player.GetComponent<Transform> ().position.y - oldLocation.y) + " This should be greater than limitY: " + limitY);
			if (Mathf.Abs (player.GetComponent<Transform> ().position.x - oldLocation.x) > Mathf.Abs (limitX)) {
				z = oldLocation.z * 1.04f;
				limitX *= 1.035f;
				Debug.LogWarning ("Zooming out in x from " + oldLocation.z + " to " + z);
			} else if (Mathf.Abs (player.GetComponent<Transform> ().position.y - oldLocation.y) > Mathf.Abs (limitY)) {
				z = oldLocation.z * 1.04f;
				limitY *= 1.035f;
				Debug.LogWarning ("Zooming out in y from " + oldLocation.z + " to " + z);
			} else {
				updateLimitX (limitX);
				updateLimitY (limitY);
				z = updateZ (oldLocation.z);
//				z = -60f;
//				Debug.LogWarning ("Reset limitX and limitY");
			}
		}

		newLocation.Set(x,y,z);
		Debug.LogWarning ("Zooming from: " + oldLocation + " to " + newLocation);
		this.GetComponent<Transform> ().position = Vector3.Lerp (oldLocation, newLocation, Time.deltaTime * zoomSpeed);
	}

	float updateLimitX (float limitX){

		if ((limitX / 1.005) < 30f) {
			return 30f;
		} else {
			return (limitX / 1.005f);
		}
	}

	float updateLimitY (float limitY){

		if ((limitY / 1.005f) < 20f) {
			return 20f;
		} else {
			return (limitY / 1.005f);
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