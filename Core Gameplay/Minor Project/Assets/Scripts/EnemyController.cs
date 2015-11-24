using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyController : NetworkBehaviour {

	public Rigidbody enemy;
	private float speed;
	private bool justflipped;
	private int flipcounter;

	private bool toe;
	private bool heel;

	void Start () {
		enemy = GetComponent<Rigidbody>();
		speed = 5;
		justflipped = false;
		flipcounter = 0;

		//toe = true;
		//heel = true;

	}

	void FixedUpdate () {

		if (justflipped) {
			float yVelocity = enemy.velocity.y;
			Vector3 movement = new Vector3 (speed, yVelocity, 0.0f); 
			enemy.velocity = movement;
			flipcounter++;
			if (flipcounter > 40){
				flipcounter = 0;
				justflipped = false;
			}
		} 
		else if (!justflipped) {
			toe = isGroundedToe ();
			Debug.Log (toe);
			heel = isGroundedHeel ();
			if (toe && heel) {
				float yVelocity = enemy.velocity.y;
				Vector3 movement = new Vector3 (speed, yVelocity, 0.0f); 
				enemy.velocity = movement;
			} 
			else {
				toe = true;
				heel = true;
				justflipped = true;
				Flip ();
			}
		}
	}

	// checks whether the front of the enemy is on a platform
	bool isGroundedToe() {
		Vector3 toePosition = new Vector3 (enemy.transform.position.x + 2.0f, enemy.transform.position.y, enemy.transform.position.z);
		return Physics.Raycast (toePosition, Vector3.down, 20f);

	}

	// checks whether the back of the enemy is on a platform
	bool isGroundedHeel() {
		Vector3 heelPosition= new Vector3 (enemy.transform.position.x - 2.0f, enemy.transform.position.y, enemy.transform.position.z);
		return Physics.Raycast (heelPosition, Vector3.down, 20f);

	}

	void Flip() {
		Debug.Log ("flipped");
		float yRotation = enemy.transform.eulerAngles.y;
		if (yRotation == 90) {
			yRotation = 270;
			speed = -5;
		} else {
			yRotation = 90;
			speed = 5;
		}		
		enemy.transform.eulerAngles = new Vector3 (270, yRotation, 0);

	}
}
