using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PatrollingGuard: NetworkBehaviour {

	private Rigidbody enemy;
	private float speed;
	private bool facingRight;

	void Start () {
		enemy = GetComponent<Rigidbody>();
		speed = 4;
		facingRight = true;
	}

	void FixedUpdate (){
		bool groundedRight = isGroundedRight ();
		bool groundedLeft = isGroundedLeft ();

		if (groundedLeft && groundedRight) {
			if (facingRight) {
				walkRight ();
				Debug.Log ("Ik loop naar rechts");
			} else {
				walkLeft ();
				Debug.Log ("Ik loop naar links");
			}
		} else if (groundedLeft && !groundedRight) {
			Debug.Log ("Ik moet nu naar links gaan lopen");
			flip ();
			walkLeft ();
			facingRight = false;
		} else if (!groundedLeft && groundedRight) {
			Debug.Log ("Ik moet nu naar rechts gaan lopen");
			flip ();
			walkRight ();
			facingRight = true;
		} else {
			Debug.Log ("Oops. Er is iets goed mis");
		}
	}

	// checks whether the rigth of the enemy is on a platform
	bool isGroundedRight() {
		Vector3 rightPosition = new Vector3 (enemy.transform.position.x + 0.9f, enemy.transform.position.y + 1, enemy.transform.position.z);
		return Physics.Raycast (rightPosition, Vector3.down, 6);
	}

	// checks whether the left of the enemy is on a platform
	bool isGroundedLeft() {
		Vector3 leftPosition= new Vector3 (enemy.transform.position.x - 0.9f, enemy.transform.position.y + 1, enemy.transform.position.z);
		return Physics.Raycast (leftPosition, Vector3.down, 6);
	}	

	void walkRight() {
		float yVelocity = enemy.velocity.y;
		Vector3 movement = new Vector3 (speed, yVelocity, 0.0f);
		enemy.velocity = movement;
	}

	void walkLeft() {
		float yVelocity = enemy.velocity.y;
		Vector3 movement = new Vector3 (speed * -1, yVelocity, 0.0f);
		enemy.velocity = movement;
	}

	void flip () {
		Vector3 theScale = enemy.transform.localScale;
		theScale.z *= -1;
		transform.localScale = theScale;
	}
}
