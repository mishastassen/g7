using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Playermini3 : NetworkBehaviour
{

    public float speed;
    public float jumpducktime;

    private float timeLeft;
    private bool jumping;
    private bool ducking;

    private Rigidbody rb;

	private Vector3 horizontalMovement;
	private miniGame3Controller minigame3controller;

	private int playerID = 1;

	//Input
	private string horizontalAxis = "Horizontal_P1";
	private string jumpButton = "Jump_P1";
	private string interact1Button = "Interact1_P1";
	private string interact2Button = "Interact2_P1";
	private string throwButton = "Throw_P1";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumping = false;
        ducking = false;
        GetComponent<Renderer>().material.color = new Color(0.5f, 1, 1);
        timeLeft = 0;
		minigame3controller = GameObject.FindGameObjectWithTag ("minigame3controller").GetComponent<miniGame3Controller> ();

		if (gameObject.GetComponent<NetworkIdentity> ().playerControllerId == 2) {
			playerID = 2;
			horizontalAxis = "Horizontal_P2";
			jumpButton = "Jump_P2";
			interact1Button = "Interact1_P2";
			interact2Button = "Interact2_P2";
			throwButton = "Throw_P2";
		}
    }

	void Update(){
		if (isLocalPlayer) {
			//timer
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				GetComponent<Renderer> ().material.color = new Color (0.5f, 1, 1);
				ducking = false;
				jumping = false;
			}

			//duck
			if (Input.GetButtonDown(interact1Button) && !ducking) {
				timeLeft = jumpducktime;
				GetComponent<Renderer> ().material.color = new Color (0.5f, 0, 1);
				ducking = true;
				jumping = false;
			}

			//jump
			if (Input.GetButtonDown(jumpButton) && !jumping) {
				timeLeft = jumpducktime;
				GetComponent<Renderer> ().material.color = new Color (0.5f, 1, 0);
				jumping = true;
				ducking = false;
			}
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
        if (other.gameObject.CompareTag("Obstacle"))
        {
            CmdLoseLife();
        }

        if (other.gameObject.CompareTag("Bridge"))
        {
            if (!ducking)
            {
                CmdLoseLife();
            }
        }
        if (other.gameObject.CompareTag("Root"))
        {
            if (!jumping)
            {
                CmdLoseLife();
            }
        }
    }

	[Command]
    void CmdLoseLife()
    {	
		minigame3controller.lives -= 1;
		minigame3controller.SetLives();
    }
}


