using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	private Animator anim;
	// private CharacterController controller;
	public float walkspeed;
	public float fallspeed;

	AudioSource geluid;
	public AudioClip wind;

	//Input
	private string horizontalAxis = "Horizontal_P1";
	private string jumpButton = "Jump_P1";
	private string interact1Button = "Interact1_P1";
	private string interact2Button = "Interact2_P1";
	private string throwButton = "Throw_P1";
	
	// walkspeed, windspeed, rotatespeed, blowdir, severity
	private float way = 1f;
	private bool falling = false;
	private float falltimer;

	// private GameObject windtoright;

	int wind_count;

	private int playerID = 1;

	private minigame2Controller minigameController;

	void Start () {
		minigameController = GameObject.FindGameObjectWithTag ("minigame2controller").GetComponent<minigame2Controller> ();
		anim = gameObject.GetComponent<Animator> ();
	
		if (gameObject.GetComponent<NetworkIdentity> ().playerControllerId == 2) {
			minigameController.left = gameObject;
			playerID = 2;
			horizontalAxis = "Horizontal_P2";
			jumpButton = "Jump_P2";
			interact1Button = "Interact1_P2";
			interact2Button = "Interact2_P2";
			throwButton = "Throw_P2";
		} else {
			minigameController.right = gameObject;
		}
	}
	

	void Update () {

		if (isLocalPlayer) {
			Vector3 PlayerPos = transform.position;
			Vector3 PlayerRot = PlayerPos - new Vector3 (0f, 0.8f, 0f);
		
			if (minigameController.windBlowing && !falling) {
				transform.RotateAround (PlayerRot, Vector3.forward, minigameController.blowdir * minigameController.severity * minigameController.windspeed * Time.deltaTime * way);
			}

			if (Input.GetButton (jumpButton) && !falling) {
				Debug.Log ("player should walk forward");
				anim.SetBool ("WalkForward", true);
				transform.Translate (Vector3.forward * walkspeed * Time.deltaTime * way);
			} else {
				anim.SetBool ("WalkForward", false);
			}
		
			if (Input.GetButton (interact1Button) && !falling) {
				transform.Translate (-Vector3.forward * walkspeed * Time.deltaTime * way);
			}

			if (Input.GetAxis (horizontalAxis) < -0.1 &&!falling) {
				transform.RotateAround (PlayerRot, Vector3.forward, 1.5f * minigameController.compensate * minigameController.windspeed * Time.deltaTime * way);
			} else if (Input.GetAxis (horizontalAxis) > 0.1 && !falling){
				transform.RotateAround (PlayerRot, Vector3.forward, -1.5f * minigameController.compensate * minigameController.windspeed * Time.deltaTime * way);
			}


			// check to see if the player falls
			if (transform.eulerAngles.z < 345 && transform.eulerAngles.z > 100 && !falling) {
				falling = true;
				anim.SetBool ("FallRight", true);
				falltimer = 1.7f; 
			}

			if (transform.eulerAngles.z > 15 && transform.eulerAngles.z < 200 && !falling) {
				falling = true;
				anim.SetBool ("FallRight", true);
				falltimer = 1.7f; 
			}

			if (falling) {
				Debug.Log (falltimer);
				if (falltimer > 0) {
					falltimer -= Time.deltaTime;
				}
				else{
					transform.Translate (Vector3.down * fallspeed * Time.deltaTime, Space.World);
				}
			}

			// has the end been reached yet?
			if (transform.position.z > 79) {
				way = 0f;
				anim.SetBool ("WalkForward", false);
			}
		}
	}
}
