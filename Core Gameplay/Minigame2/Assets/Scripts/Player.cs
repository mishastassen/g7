using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Animator leftanim;
	public Animator rightanim;
	// private CharacterController controller;
	public float walkspeed;

	AudioSource geluid;
	public AudioClip wind;
	
	public GameObject left;
	public GameObject right;
	
	// walkspeed, windspeed, rotatespeed, blowdir, severity
	public float windspeed;
	private float fallspeed;
	private float blowdir;
	private float severity;
	private float compensate;
	private float way_left;
	private float way_right;
	private bool falling_left;
	private bool falling_right;
	
	// is the wind blowing?
	private bool windBlowing;
	public ParticleSystem toleft;
	public ParticleSystem toright;
	// private GameObject windtoright;
	
	int frame_count;
	int wind_count;

	// Use this for initialization
	void Start () {
		severity = 1.0f;
		fallspeed = 3.0f;
		compensate = 1.5f;
		frame_count = 1;
		blowdir = getWindDirection ();

		toleft.enableEmission = false;
		toright.enableEmission = false;

		way_left = 1f;
		way_right = 1f;

		falling_left = false;
		falling_right = false;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 LeftPlayerPos = left.transform.position;
		Vector3 LeftPlayerRot = LeftPlayerPos - new Vector3(0f,0.8f,0f);

		Vector3 RightPlayerPos = right.transform.position;
		Vector3 RightPlayerRot = RightPlayerPos - new Vector3 (0f, 0.8f, 0f);

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
			
			left.transform.RotateAround (LeftPlayerRot, Vector3.forward, blowdir * severity * windspeed * Time.deltaTime * way_left);
			right.transform.RotateAround (RightPlayerRot, Vector3.forward, blowdir * severity * windspeed * Time.deltaTime * way_right);
			
			if (wind_count > 40) {
				windBlowing = false;
				wind_count = 0;
				blowdir = getWindDirection();
				// geluid.Stop ();
				toleft.enableEmission = false;
				toright.enableEmission = false;
			}
		}

		if (Input.GetKey (KeyCode.W)) {
			leftanim.SetBool ("WalkForward", true);
			left.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime * way_left);
		} else {
			leftanim.SetBool ("WalkForward", false);
		}
		
		if (Input.GetKey (KeyCode.S)) {
			leftanim.SetInteger ("WalkBackwards", 1);
			left.transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime * way_left);
		} else {
			leftanim.SetInteger ("WalkBackwards", 0);
		}

		if (Input.GetKey (KeyCode.A)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, 1.5f * compensate*windspeed*Time.deltaTime * way_left);
		}
		
		if (Input.GetKey (KeyCode.D)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, -1.5f * compensate*windspeed*Time.deltaTime * way_left);
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			rightanim.SetBool ("WalkForward", true);
			right.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime * way_right);
		} else {
			rightanim.SetBool ("WalkForward", false);
		}
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			rightanim.SetInteger ("WalkBackwards", 1);
			right.transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime * way_right);
		} else {
			rightanim.SetInteger ("WalkBackwards", 0);
		}
		
		if (Input.GetKey (KeyCode.LeftArrow)) {
			right.transform.RotateAround(RightPlayerRot,Vector3.forward, 1.5f * compensate*windspeed*Time.deltaTime * way_right);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			right.transform.RotateAround(RightPlayerRot,Vector3.forward, -1.5f * compensate*windspeed*Time.deltaTime * way_right);
		}

		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.UpArrow)) {
			leftanim.SetBool ("WalkForward", true);
			left.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime * way_left);
			rightanim.SetBool ("WalkForward", true);
			right.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime * way_right);
		}

		// increase the severity of the wind if one progresses
		if (left.transform.position.z > 10 | right.transform.position.z > 10) {
			severity = 1.25f;
			compensate = severity * 1.4f;
		}
		
		if (left.transform.position.z > 20 | right.transform.position.z > 20) {
			severity = 1.5f;
			compensate = severity * 1.3f;
		}
		
		if (left.transform.position.z > 30 | right.transform.position.z > 30) {
			severity = 2.0f;
			compensate = severity * 1.1f;
		}

		// check to see when the left player falls
		if (left.transform.eulerAngles.z < 345 && left.transform.eulerAngles.z > 100) {
			falling_left = true;
			leftanim.SetBool ("FallRight" ,true);
		}

		if (left.transform.eulerAngles.z > 15 && left.transform.eulerAngles.z < 200) {
			falling_left = true;
			leftanim.SetBool ("FallRight" ,true);
		}

		if (falling_left) {
			left.transform.Translate( Vector3.down * fallspeed * Time.deltaTime);
		}

		// check to see when the right player falls 
		if (right.transform.eulerAngles.z < 345 && right.transform.eulerAngles.z > 100) {
			falling_right = true;
			rightanim.SetBool ("FallRight" ,true);
		}
		
		if (right.transform.eulerAngles.z > 15 && right.transform.eulerAngles.z < 200) {
			falling_right = true;
			rightanim.SetBool ("FallRight" ,true);
		}

		if (falling_right) {
			right.transform.Translate( Vector3.down * fallspeed * Time.deltaTime);
		}

		// has the end been reached yet?
		if (left.transform.position.z > 39) {
			way_left = 0f;
		}

		if (right.transform.position.z > 39) {
			way_right = 0f;
		}
		// end of update function below
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
		return blowdir;
	}
}
