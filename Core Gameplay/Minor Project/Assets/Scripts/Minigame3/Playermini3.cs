using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Playermini3 : NetworkBehaviour
{

    public float speed;
    public float jumpducktime;

    private float timeLeft;
    private bool jumping;
    private bool ducking;
	[SyncVar(hook="OnAnimationChange")]
	private int animationID = 0;

    private Rigidbody rb;
	private Animator anim;

	private Vector3 horizontalMovement;
	private miniGame3Controller minigame3controller;

	private int playerID = 1;

	//Input
	private string horizontalAxis = "Horizontal_P1";
	private string jumpButton = "Jump_P1";
	private string interact1Button = "Interact1_P1";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator> ();
        jumping = false;
        ducking = false;
        timeLeft = 0;
		minigame3controller = GameObject.FindGameObjectWithTag ("minigame3controller").GetComponent<miniGame3Controller> ();

		if (gameObject.GetComponent<NetworkIdentity> ().playerControllerId == 2) {
			playerID = 2;
			horizontalAxis = "Horizontal_P2";
			jumpButton = "Jump_P2";
			interact1Button = "Interact1_P2";
		}
    }

	void Update(){
		if (isLocalPlayer) {
			//timer
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				ducking = false;
				jumping = false;
			}

        

			//duck
			if (Input.GetButtonDown(interact1Button) && !ducking) {
				timeLeft = jumpducktime;
				ducking = true;
				jumping = false;
			}

			//duck stop
			if (Input.GetButtonUp(interact1Button)) {
				timeLeft = jumpducktime;
				ducking = false;
				jumping = false;
			}

			//jump
			if (Input.GetButtonDown(jumpButton) && !jumping) {
				timeLeft = jumpducktime;
				jumping = true;
				ducking = false;
			}

			//jump stop
			if (Input.GetButtonUp(jumpButton)) {
				timeLeft = jumpducktime;
				jumping = false;
				ducking = false;
			}


			int state = ducking?-1:jumping?1:0;
			CmdCheckAnimation(state);
		}
	}
		
    void FixedUpdate()
    {
		if (isLocalPlayer) {
			// horizontal movement
			float moveHorizontal = Input.GetAxis (horizontalAxis);
			Vector3 horizontalMovement = new Vector3 (moveHorizontal, 0.0f, 0.0f);
			rb.transform.Translate (horizontalMovement * speed);
			horizontalMovement = Vector3.zero;
		}
    }

    void OnTriggerEnter(Collider other)
    {
		if (isLocalPlayer) {
			if (other.gameObject.CompareTag ("Obstacle")) {
				CmdLoseLife ();
			}

			if (other.gameObject.CompareTag ("Bridge")) {
				if (!ducking) {
					CmdLoseLife ();
				}
			}

			if (other.gameObject.CompareTag ("Root")) {
				if (!jumping) {
					CmdLoseLife ();
				}
			}
		}
    }

	[Command]
    void CmdLoseLife()
    {	
		minigame3controller.lives -= 1;
		minigame3controller.SetLives();
    }

	[Command]
	void CmdCheckAnimation(int state) {
		animationID = state;
	}

	void OnAnimationChange(int state) {
		anim.SetInteger ("animationID", state);
		this.animationID = state;
	}

}


