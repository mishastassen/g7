using UnityEngine;
using UnityEngine.Networking;

public class playerscript : NetworkBehaviour {
	
	public float acceleration; //acceleration of ball

	//Server movement vector
	Vector3 movement;

	//Client movement command throttling
	Vector3 oldMovement;

	//Initalization
	void Start () {
		if (!NetworkServer.active) {
			//GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	// Update is called once per frame
	void Update() 
	{
		if (NetworkServer.active)	//Checks if this is object is on a server
			UpdateServer();
		
		if (NetworkClient.active)   //Checks if this object is on a client
			UpdateClient();
	}

	//Update function for client
	void UpdateClient () {
		if (!isLocalPlayer) { //Check if this is the player corresponding with the local client if not return
			return;
		}
		//Process control inputs for local player
		float ver =  Input.GetAxis("Vertical");
		float hor = Input.GetAxis("Horizontal");

		if (ver > 0) { ver = 1; }
		if (ver < 0) { ver = -1; }
		if (hor > 0) { hor = 1; }
		if (hor < 0) { hor = -1; }
		
		Vector3 movForce = new Vector3 (hor, 0, ver);
		
		//CmdMove (movForce);
		GetComponent<Rigidbody> ().AddForce (movForce*acceleration);

	}

	//Update function for client
	void UpdateServer()
	{
	//	GetComponent<Rigidbody> ().AddForce (this.movement*acceleration);
	}

	//Set movement of ball on server
	[Command]
	void CmdMove(Vector3 movForce)
	{
	//	this.movement = movForce;
	}

}
