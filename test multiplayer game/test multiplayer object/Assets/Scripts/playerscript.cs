using UnityEngine;
using UnityEngine.Networking;

public class playerscript : NetworkBehaviour {
	
	public float acceleration; //acceleration of ball

	public GameObject boxprefab;

	//Server movement vector
	Vector3 movement;

	//Client movement command throttling
	Vector3 oldMovement;

	//Initalization
	void Start () {
		if (isLocalPlayer) {
			this.GetComponent<Renderer>().material.color = Color.green;
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
		handleMovement();

		//Process other input for local player
		if(Input.GetKeyDown("space")){
			CmdCreateBox();
		}

	}
	
	//Handle movement of the player
	void handleMovement(){
		float ver =  Input.GetAxis("Vertical");
		float hor = Input.GetAxis("Horizontal");
		
		if (ver > 0) { ver = 1; }
		if (ver < 0) { ver = -1; }
		if (hor > 0) { hor = 1; }
		if (hor < 0) { hor = -1; }
		
		Vector3 movForce = new Vector3 (hor, 0, ver);
		
		GetComponent<Rigidbody> ().AddForce (movForce*acceleration);

	}


	//Update function for client
	void UpdateServer()
	{
		
	}

	//Set movement of ball on server
	[Command]
	void CmdCreateBox()
	{
		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = Vector3.zero;
		GameObject box = (GameObject)Instantiate (boxprefab, new Vector3 (0, 0, 0), rotation);
		NetworkServer.Spawn (box);
	}

}
