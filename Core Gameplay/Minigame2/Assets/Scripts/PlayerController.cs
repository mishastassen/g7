using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	AudioSource geluid;
	public AudioClip wind;

	public Rigidbody left;
	public Rigidbody right;

	// walkspeed, windspeed, rotatespeed, blowdir, severity
	public float walkspeed;
	public float windspeed;
	public int rotatespeed;
	private float blowdir;
	private float severity;
	private float compensate;

	// is the wind blowing?
	private bool windBlowing;

	int frame_count;
	int wind_count;

	// set important values
	void Start () {
		windBlowing = false;
		frame_count = 0;
		wind_count = 0;
		left.GetComponent<Rigidbody> ();
		geluid = GetComponent<AudioSource>();
		blowdir = getWindDirection ();
		severity = 1.0f;
		compensate = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		frame_count++;

		if (frame_count % 100 == 0) {
			windBlowing = true;
		}

		if (windBlowing) {
			Debug.Log("wind blowing");

			// geluid.PlayOneShot(wind,1.0f);
			wind_count ++;

			// add the force due to the wind
			//rb.AddForce (Vector3.left * windspeed * blowdir * severity);

			Vector3 LeftPlayerPos = left.transform.position;
			Vector3 LeftPlayerRot = LeftPlayerPos - new Vector3(0f,0.8f,0f);

			left.transform.RotateAround(LeftPlayerRot,Vector3.forward,blowdir * severity * windspeed*Time.deltaTime);

			if (wind_count > 40) {
				windBlowing = false;
				wind_count = 0;
				blowdir = getWindDirection();
				geluid.Stop();
			}
		}

		// increase the severity of the wind if one progresses
		if (transform.position.z > 10) {
			severity = 1.25f;
			compensate = severity * 1.4f;
		}

		if (transform.position.z > 20) {
			severity = 1.5f;
			compensate = severity * 1.3f;
		}

		if (transform.position.z > 30) {
			severity = 2.0f;
			compensate = severity * 1.1f;
		}
	}

	// the controls
	void FixedUpdate(){

		Vector3 LeftPlayerPos = left.transform.position;
		Vector3 LeftPlayerRot = LeftPlayerPos - new Vector3(0f,0.8f,0f);

		if (Input.GetKey (KeyCode.UpArrow)) {
			left.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			left.transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, 1.5f * compensate*windspeed*Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, -1.5f * compensate*windspeed*Time.deltaTime);
		}
	}

	// returns the blowdirection of the wind. Random, as you can see
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
