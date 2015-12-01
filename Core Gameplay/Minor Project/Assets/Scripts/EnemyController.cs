using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyController : NetworkBehaviour {

	private Rigidbody enemy;
	private float speed;
	private bool facingRight;

	void Start () {
		enemy = GetComponent<Rigidbody>();
		speed = 4;
		facingRight = true;
	}

	void FixedUpdate() {
		bool groundedRight = isGroundedRight ();
		bool groundedLeft = isGroundedLeft ();
		if (groundedRight && groundedLeft) {
			// keep walking
		} else if (groundedRight) {
			if (!facingRight) {
				flip ();
			}
			facingRight = true;
		} else if (groundedLeft) {
			if (facingRight) {
				flip ();
			}
			facingRight = false;
		}
		if (facingRight) {
			walkRight ();
		} else {
			walkLeft ();
		}
	}

	// checks whether the rigth of the enemy is on a platform
	bool isGroundedRight() {
		Vector3 rightPosition = new Vector3 (enemy.transform.position.x + 0.5f, enemy.transform.position.y + 1, enemy.transform.position.z);
		Debug.DrawRay (rightPosition, Vector3.down, Color.red);
		return Physics.Raycast (rightPosition, Vector3.down, 2);
	}

	// checks whether the left of the enemy is on a platform
	bool isGroundedLeft() {
		Vector3 leftPosition= new Vector3 (enemy.transform.position.x - 0.5f, enemy.transform.position.y + 1, enemy.transform.position.z);
		Debug.DrawRay (leftPosition, Vector3.down, Color.blue);
		return Physics.Raycast (leftPosition, Vector3.down, 2);
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
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}
}
