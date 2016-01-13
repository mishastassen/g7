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
	private float zoomSpeed = 2f;
	private float zoomMin = 25f;
	private float zoomMax = 45f;

	private string currentLevel;
	private GameObject deliveryZone;
	private bool ready;
	private bool isStarted = false;

	private float lerpTime;
	private float currentLerpTime;
	private Vector3 playerPos;

	private float currentLerpTimeX;
	private float lerpTimeX;
	private bool zooming;

	private float x;
	private float y;
	private float z0;
	private float z1;

	// Use this for initialization
	void Start () {
		zooming = false;
		ready = false;
		currentLevel = Gamevariables.currentLevel;
		players = new List<GameObject>();
		windowCenter.x = this.GetComponent<Transform> ().position.x;
		windowCenter.y = this.GetComponent<Transform> ().position.y;
		previousCenter = windowCenter;
		limitX = 30f;
		limitY = 10f;
		cam = this.GetComponent<Camera> ();
		zoom = cam.fieldOfView;
		lerpTime = 15f;
		playerPos = (this.GetComponent<Transform> ().position);
		if (currentLevel == "Level6") {
			StartCoroutine (lerpCamera ());
		} else {
			ready = true;
		}
		lerpTimeX = 2f;
		currentLerpTimeX = 0f;
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
			} else {
				deliveryZone = GameObject.FindGameObjectWithTag ("DeliveryZone");
				Vector3 delivPos = (deliveryZone.GetComponent<Transform> ().position);
				delivPos.z = -80f;
				currentLerpTime += Time.deltaTime;
				cam.fieldOfView = 40;
				playerPos.z = -80f;
				if (currentLerpTime > lerpTime) {
					currentLerpTime = lerpTime;
				}
				float perc = currentLerpTime / lerpTime;
				this.GetComponent<Transform> ().position = Vector3.Lerp (delivPos, playerPos, perc);
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
		newLocation.Set(windowCenter.x, windowCenter.y + 1.0f, -60.0f);
		previousCenter = new Vector2 (center.x, center.y);

		foreach (GameObject player in players) {
			if (Mathf.Abs (player.GetComponent<Transform> ().position.x - newLocation.x) > Mathf.Abs(limitX)) {
				float x = newLocation.x;
				float y = newLocation.y;
				float z0 = newLocation.z;
				float z1 = newLocation.z * 1.1f;

				oldLocation.Set (x, y, z0);
				newLocation.Set (x, y, z1);

				currentLerpTimeX += Time.deltaTime;

				if (currentLerpTimeX > lerpTimeX) {
					currentLerpTimeX = lerpTimeX;
				}
				float perc = currentLerpTimeX / lerpTimeX;
				Debug.Log (perc);

				this.GetComponent<Transform> ().position = Vector3.Lerp (oldLocation, newLocation, perc);

				if (!zooming) {
					StartCoroutine (busyZooming ());
				}
				// cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, cam.fieldOfView*1.05f, Time.deltaTime * zoomSpeed);
				limitX *= (player.GetComponent<Transform> ().position.x / limitX);
			} else if (Mathf.Abs (player.GetComponent<Transform>().position.y - newLocation.y) > Mathf.Abs(limitY)) {
				cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, cam.fieldOfView*1.15f, Time.deltaTime * zoomSpeed);
				limitY *= (player.GetComponent<Transform> ().position.y / limitY);
				Debug.Log (limitY);
			}
			else {
				limitX = 30f;
				limitY = 10f;
				cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,35,Time.deltaTime*(zoomSpeed/2f));
			}
		}
		// this.GetComponent<Transform> ().position = newLocation;
	}

	IEnumerator busyZooming (){
		zooming = true;
		yield return new WaitForSeconds(lerpTimeX);
		currentLerpTimeX = 0f;
		zooming = false;
	}

	IEnumerator lerpCamera (){
		yield return new WaitForSeconds(15f);
		ready = true;
	}
}