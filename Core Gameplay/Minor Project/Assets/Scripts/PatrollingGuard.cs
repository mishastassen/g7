using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PatrollingGuard: NetworkBehaviour {

	private Rigidbody enemy;
	private float speed;
	private bool facingRight;

	//private Vector3 eyePosition;
	private bool spotted;
	private bool enabled;
	//private float coneDegrees; //halve hoek van gezichtspunt gegeven in graden
	//private Vector3 direction;
	//private float angle;
	//RaycastHit hitInfo;

	void Start () {
		enemy = GetComponent<Rigidbody>();
		speed = 4;
		facingRight = true;
	}
	/*
	void OnEnable () {
		Eventmanager.Instance.EventonPlayerDeath += HandleEventonPlayerDeath;
		enabled = true;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
	}

	void OnDisable () {
		if (enabled) {
			Eventmanager.Instance.EventonPlayerDeath -= HandleEventonPlayerDeath;
			enabled = false;
		}
	}
	*/
	void FixedUpdate (){
		bool groundedRight = isGroundedRight ();
		bool groundedLeft = isGroundedLeft ();

		if (groundedLeft && groundedRight) {
			if (facingRight) {
				walkRight ();
				//Debug.Log ("Ik loop naar rechts");
			} else {
				walkLeft ();
				//Debug.Log ("Ik loop naar links");
			}
		} else if (groundedLeft && !groundedRight) {
			//Debug.Log ("Ik moet nu naar links gaan lopen");
			flip ();
			walkLeft ();
			facingRight = false;
		} else if (!groundedLeft && groundedRight) {
			//Debug.Log ("Ik moet nu naar rechts gaan lopen");
			flip ();
			walkRight ();
			facingRight = true;
		} else {
			//Debug.Log ("Oops. Er is iets goed mis");
		}
		Debug.Log (spotted);
		if (spotted) {
			CmdPlayerSpotted ();
		} 
		else {
			CmdNoPlayerSpotted ();
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

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			spotted = true;
			/*
			Debug.Log ("player seen");
			direction = other.transform.position + new Vector3(0,3,0) - eyePosition;
			Debug.DrawRay (eyePosition, direction, Color.blue);
			if (facingRight) {
				angle = Vector3.Angle (direction, Vector3.right);
				direction = Vector3.right;
			} else {
				angle = Vector3.Angle (direction, Vector3.left);
				direction = Vector3.left;
			}
			// Debug.Log ("Angle " + angle + " condeDeg " + coneDegrees);

			if (angle <= coneDegrees) {
				Debug.Log ("Drawing Line!");
				Debug.DrawRay(eyePosition,direction*1000f, Color.red, duration:5f);
				if (Physics.Raycast (eyePosition, direction, out hitInfo)) {
					Debug.Log (hitInfo.collider.tag);
					if (hitInfo.collider.tag == "Player") {
						Debug.Log ("Hit by Player");
						Debug.DrawRay(eyePosition, direction, Color.red);
						spotted = true;
					} else {
						Debug.Log ("Did not hit the Player but hit " + hitInfo.collider.name);
						Debug.DrawRay(eyePosition, direction, Color.green);
						spotted = false;
					}
				} else {
					Debug.DrawRay(eyePosition, direction, Color.green);
					spotted = false;
				}
			} else {
				Debug.DrawRay(eyePosition, direction, Color.green);
				spotted = false;
			}
			*/
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			spotted = false;
		}
	}
	/*
	void HandleEventonPlayerDeath (GameObject player) {
		spotted = false;
		Gamevariables.alarmPercent = -1;
	}
	*/
	[Command]
	void CmdPlayerSpotted() {
		Eventmanager.Instance.triggerPlayerSpotted();
	}

	[Command]
	void CmdNoPlayerSpotted() {
		Eventmanager.Instance.triggerNoPlayerSpotted ();
	}
}
