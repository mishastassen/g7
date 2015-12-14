using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GuardContoller : NetworkBehaviour {

	private Rigidbody enemy;
	private Vector3 eyePosition;

	private bool finished;
	private bool facingRight;
	private bool spotted;

	private float coneDegrees; //halve hoek van gezichtspunt gegeven in graden

	private Vector3 direction;
	private float angle;
	RaycastHit hitInfo;

	void Start () {
		enemy = GetComponent<Rigidbody> ();
		eyePosition = new Vector3 (enemy.position.x + 0.5f, enemy.position.y + 6.0f, enemy.position.z);
		finished = true;
		facingRight = true;
		spotted = false;
		coneDegrees = 30;
	}
	
	void FixedUpdate () {
		if (finished) {
			StartCoroutine (flipGuard ());
			finished = false;
			facingRight = !facingRight;
		}

		if (facingRight) {
			eyePosition = new Vector3 (enemy.position.x + 0.5f, enemy.position.y + 6.0f, enemy.position.z);
		} else {
			eyePosition = new Vector3 (enemy.position.x - 0.5f, enemy.position.y + 6.0f, enemy.position.z);
		}

		if (spotted) {
			CmdPlayerSpotted ();
		} else {
			CmdNoPlayerSpotted ();
		}
	}

	IEnumerator flipGuard () {
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
		yield return new WaitForSeconds(5);
		finished = true;
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			direction = other.transform.position + new Vector3(0,3,0) - eyePosition;
			if (facingRight) {
				angle = Vector3.Angle (direction, Vector3.right);
			} else {
				angle = Vector3.Angle (direction, Vector3.left);
			}

			if (angle <= coneDegrees) {
				if (Physics.Raycast (eyePosition, direction, out hitInfo)) {
					Debug.Log (hitInfo.collider.tag);
					if (hitInfo.collider.tag == "Player") {
						Debug.DrawRay(eyePosition, direction, Color.red);
						spotted = true;
					} else {
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
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			spotted = false;
		}
	}

	[Command]
	void CmdPlayerSpotted() {
		Eventmanager.Instance.triggerPlayerSpotted();
	}

	[Command]
	void CmdNoPlayerSpotted() {
		Eventmanager.Instance.triggerNoPlayerSpotted ();
	}

}
