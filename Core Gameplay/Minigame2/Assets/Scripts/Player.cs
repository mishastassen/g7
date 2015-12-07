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
	public ParticleSystem toleft;
	public ParticleSystem toright;
	// private GameObject windtoright;
	
	int frame_count;
	int wind_count;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<Animator> ();
		controller = GetComponent<CharacterController> ();
		severity = 1.0f;
		compensate = 1.5f;
		frame_count = 1;
		blowdir = getWindDirection ();
		toleft.enableEmission = false;
		toright.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 LeftPlayerPos = left.transform.position;
		Vector3 LeftPlayerRot = LeftPlayerPos - new Vector3(0f,0.8f,0f);

		frame_count++;
		
		if (frame_count % 100 == 0) {
			windBlowing = true;
		}
		
		if (windBlowing) {
			// geluid.PlayOneShot(wind,1.0f);
			wind_count ++;

			if (blowdir > 0){
				toleft.enableEmission = true;
				toleft.Play();
			} else{
				toright.enableEmission = true;
				toright.Play ();
			}
			
			left.transform.RotateAround (LeftPlayerRot, Vector3.forward, blowdir * severity * windspeed * Time.deltaTime);
			
			if (wind_count > 40) {
				windBlowing = false;
				wind_count = 0;
				blowdir = getWindDirection();
				// geluid.Stop ();
				toleft.enableEmission = false;
				toright.enableEmission = false;
			}
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			anim.SetBool ("WalkForward", true);
			left.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
		} else {
			anim.SetBool ("WalkForward", false);
		}
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			anim.SetInteger ("WalkBackwards", 1);
			left.transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime);
		} else {
			anim.SetInteger ("WalkBackwards", 0);
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, 1.5f * compensate*windspeed*Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.RightArrow)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, -1.5f * compensate*windspeed*Time.deltaTime);
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

		// check to see when to fall left
		if (left.transform.eulerAngles.z < 345 && left.transform.eulerAngles.z > 100) {
			anim.SetBool ("FallRight" ,true);
			left.transform.Translate( Vector3.down * 3 * Time.deltaTime);
		}

		if (left.transform.eulerAngles.z > 15 && left.transform.eulerAngles.z < 200) {
			anim.SetBool ("FallRight" ,true);
			left.transform.Translate( Vector3.down * 3 * Time.deltaTime);
		}
	}

	float getWindDirection(){
		blowdir = Random.Range (-1.0f, 1.0f);
		
		if (blowdir < 0) {
			Debug.Log ("wind should be blowing");
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
