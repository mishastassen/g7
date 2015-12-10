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
		// leftanim = gameObject.GetComponentInChildren<Animator> ();
		// rightanim = gameObject.GetComponentInChildren<Animator> ();
		// controller = GetComponent<CharacterController> ();
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
			
			left.transform.RotateAround (LeftPlayerRot, Vector3.forward, blowdir * severity * windspeed * Time.deltaTime);
			right.transform.RotateAround (RightPlayerRot, Vector3.forward, blowdir * severity * windspeed * Time.deltaTime);
			
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
			left.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
		} else {
			leftanim.SetBool ("WalkForward", false);
		}
		
		if (Input.GetKey (KeyCode.S)) {
			leftanim.SetInteger ("WalkBackwards", 1);
			left.transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime);
		} else {
			leftanim.SetInteger ("WalkBackwards", 0);
		}

		if (Input.GetKey (KeyCode.A)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, 1.5f * compensate*windspeed*Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.D)) {
			left.transform.RotateAround(LeftPlayerRot,Vector3.forward, -1.5f * compensate*windspeed*Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			rightanim.SetBool ("WalkForward", true);
			right.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
		} else {
			rightanim.SetBool ("WalkForward", false);
		}
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			rightanim.SetInteger ("WalkBackwards", 1);
			right.transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime);
		} else {
			rightanim.SetInteger ("WalkBackwards", 0);
		}
		
		if (Input.GetKey (KeyCode.LeftArrow)) {
			right.transform.RotateAround(RightPlayerRot,Vector3.forward, 1.5f * compensate*windspeed*Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			right.transform.RotateAround(RightPlayerRot,Vector3.forward, -1.5f * compensate*windspeed*Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.UpArrow)) {
			leftanim.SetBool ("WalkForward", true);
			left.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
			rightanim.SetBool ("WalkForward", true);
			right.transform.Translate (Vector3.forward * walkspeed * Time.deltaTime);
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

		// check to see when the left player falls
		if (left.transform.eulerAngles.z < 345 && left.transform.eulerAngles.z > 100) {
			leftanim.SetBool ("FallRight" ,true);
			left.transform.Translate( Vector3.down * 3 * Time.deltaTime);
		}

		if (left.transform.eulerAngles.z > 15 && left.transform.eulerAngles.z < 200) {
			leftanim.SetBool ("FallRight" ,true);
			left.transform.Translate( Vector3.down * 3 * Time.deltaTime);
		}

		// check to see when the right player falls
		if (right.transform.eulerAngles.z < 345 && right.transform.eulerAngles.z > 100) {
			rightanim.SetBool ("FallRight" ,true);
			right.transform.Translate( Vector3.down * 3 * Time.deltaTime);
		}
		
		if (right.transform.eulerAngles.z > 15 && right.transform.eulerAngles.z < 200) {
			rightanim.SetBool ("FallRight" ,true);
			right.transform.Translate( Vector3.down * 3 * Time.deltaTime);
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
