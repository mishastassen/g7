using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator anim;
	private CharacterController controller;
	public float walkspeed;

	AudioSource geluid;
	public AudioClip wind;
	
	public GameObject left;
	public GameObject right;
	
	// walkspeed, windspeed, rotatespeed, blowdir, severity
	public float windspeed;
	private float blowdir;
	private float severity;
	private float compensate;
	
	// is the wind blowing?
	private bool windBlowing;
	
	int frame_count;
	int wind_count;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<Animator> ();
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("should not move forward");

		Vector3 LeftPlayerPos = left.transform.position;
		Vector3 LeftPlayerRot = LeftPlayerPos - new Vector3(0f,0.8f,0f);
		
		if (Input.GetKey (KeyCode.UpArrow)) {
			left.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
			Debug.Log("should move forward");
		}
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			left.transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime);
			Debug.Log("should move backward");
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, 1.5f * compensate*windspeed*Time.deltaTime);
			Debug.Log("should turn left");
		}
		
		if (Input.GetKey (KeyCode.RightArrow)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, -1.5f * compensate*windspeed*Time.deltaTime);
			Debug.Log("should turn right");
		}
	}

	float getWindDirection(){
		blowdir = Random.Range (-1.0f, 1.0f);
		
		if (blowdir < 0) {
			blowdir = -1;
		} else if (blowdir > 0) {
			blowdir = 1;
		} else {
			blowdir = getWindDirection();
		}
		// blowdir = 0;
		return blowdir;
	}
}
