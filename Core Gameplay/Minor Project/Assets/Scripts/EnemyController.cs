using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyController : NetworkBehaviour {

	public Rigidbody enemy;
	private float speed;

	Vector3 middle;
	float distance;

	void Start () {
		enemy = GetComponent<Rigidbody>();
		speed = 4;
		middle = enemy.transform.position;
	}

	void Update () {
		distance = Vector3.Distance (middle, enemy.transform.position);
		if (distance < 9.5f) {
			walkNormal ();
		} else {
			flip ();

			walkNormal ();
			walkNormal ();
			walkNormal ();
		}

		//Debug.Log ("Toe  " + isGroundedToe ());
		//Debug.Log ("Heel " + isGroundedHeel ());
	}

	void walkNormal (){
		enemy.transform.Translate (Vector3.down * speed * Time.deltaTime);
	}

	// checks whether the front of the enemy is on a platform
	bool isGroundedToe() {
		Vector3 toePosition = new Vector3 (enemy.transform.position.x , enemy.transform.position.y, enemy.transform.position.z);
		return Physics.Raycast (toePosition, Vector3.up, 3.0f);
	}

	// checks whether the back of the enemy is on a platform
	bool isGroundedHeel() {
		Vector3 heelPosition= new Vector3 (enemy.transform.position.x , enemy.transform.position.y, enemy.transform.position.z);
		return Physics.Raycast (heelPosition, Vector3.up, 3.0f);
	}

	void flip () {
		float yRotation = enemy.transform.eulerAngles.y;
		if (yRotation == 90) {
			yRotation = 270;
		} else {
			yRotation = 90;
		}		
		enemy.transform.eulerAngles = new Vector3 (270, yRotation, 0);
	}
}
