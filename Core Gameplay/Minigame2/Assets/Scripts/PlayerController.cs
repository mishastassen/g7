using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Rigidbody rb;

	// walkspeed, windspeed, rotatespeed, blowdir, severity
	public float walkspeed;
	public float windspeed;
	public int rotatespeed;
	private float blowdir;
	private float severity;

	// is the wind blowing?
	private bool windBlowing;

	int frame_count;
	int wind_count;

	// set important values
	void Start () {
		windBlowing = false;
		frame_count = 0;
		wind_count = 0;
		rb.GetComponent<Rigidbody> ();
		blowdir = getWindDirection();
		severity = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		frame_count++;

		if (frame_count % 100 == 0) {
			windBlowing = true;
		}

		if (windBlowing) {

			wind_count ++;

			// add the force due to the wind
			rb.AddForce (Vector3.left * windspeed * blowdir * severity);

			if (wind_count > 40) {
				windBlowing = false;
				wind_count = 0;
				blowdir = getWindDirection();
			}
		}

		// increase the severity of the wind if one progresses
		if (transform.position.z > 5) {
			severity = 1.25f;
		}

		if (transform.position.z > 10) {
			severity = 1.5f;
		}

		if (transform.position.z > 15) {
			severity = 2.0f;
		}

	}


	// the controls
	void FixedUpdate()
	{
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			rb.AddForce(Vector3.left * rotatespeed);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			rb.AddForce(-Vector3.left * rotatespeed);
		}
	}

	// returns the blowdirection of the wind. Random, as you can see
	float getWindDirection(){
		blowdir = Random.Range (-1.0f, 1.0f);

		if (blowdir < 0) {
			blowdir = -1;
		} else {
			blowdir = 1;
		}
		return blowdir;
		}
}
