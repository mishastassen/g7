using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyController : NetworkBehaviour {

	private Rigidbody enemy;
	private float speed;

	void Start () {
		enemy = GetComponent<Rigidbody>();
		speed = 5;
	}

	void FixedUpdate () {		
		if (isGroundedToe() && isGroundedHeel()) {
			float yVelocity = enemy.velocity.y;
			Vector3 movement = new Vector3 (speed, yVelocity, 0.0f); 
			enemy.velocity = movement;
		} else {
			Flip ();
		}
	}

	// checks whether the front of the enemy is on a platform
	bool isGroundedToe() {
		Vector3 toePosition = new Vector3(enemy.transform.position.x + 2.0f, enemy.transform.position.y, enemy.transform.position.z);
		return Physics.Raycast (toePosition, -Vector3.up, 0.1f);
	}

	// checks whether the back of the enemy is on a platform
	bool isGroundedHeel() {
		Vector3 heelPosition = new Vector3(enemy.transform.position.x - 2.0f, enemy.transform.position.y, enemy.transform.position.z);
		return Physics.Raycast (heelPosition, -Vector3.up, 0.1f);
	}

	void Flip() {		
		enemy.transform.Translate(new Vector3(0,1,0));
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
