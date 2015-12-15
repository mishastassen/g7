using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GuardContoller : NetworkBehaviour {

	public GameObject enemy;
	private Vector3 eyePosition;

	private bool finished;
	private bool facingRight;
	private bool spotted;
	public float searchTime = 5f;
	private bool searching;

	private float coneDegrees; //halve hoek van gezichtspunt gegeven in graden

	private Vector3 direction;
	private float angle;
	RaycastHit hitInfo;

	NavMeshAgent agent;
	private Transform PlayerPos;
	public Transform SpawnLocation;

	void Start () {
		// enemy = GetComponent<Rigidbody> ();
		eyePosition = new Vector3 (enemy.transform.position.x + 0.5f, enemy.transform.position.y + 6.0f, enemy.transform.position.z);
		finished = true;
		searching = false;
		facingRight = false;
		spotted = false;
		coneDegrees = 45;
		agent = GetComponentInParent<NavMeshAgent> ();
		agent.destination = SpawnLocation.position;
	}
	
	void FixedUpdate () {

		if (facingRight) {
			eyePosition = new Vector3 (enemy.transform.position.x + 0.5f, enemy.transform.position.y + 6.0f, enemy.transform.position.z);
		} else {
			eyePosition = new Vector3 (enemy.transform.position.x - 0.5f, enemy.transform.position.y + 6.0f, enemy.transform.position.z);
		}

		if (spotted) {
			CmdPlayerSpotted ();
			Debug.Log ("Ik ben hier!");
			agent.destination = PlayerPos.position;
		} 
		else {
			CmdNoPlayerSpotted ();
			StartCoroutine (wait ());
			agent.destination = SpawnLocation.position;
			Debug.Log ("Ik ben hier niet!");
			if (finished) {
				StartCoroutine (flipGuard ());
				finished = false;
				facingRight = !facingRight;
			}
		}
	}

	IEnumerator wait (){
		agent.destination = PlayerPos.position;
		yield return new WaitForSeconds (5);
	}


	IEnumerator flipGuard () {
		yield return new WaitForSeconds(5);
		Vector3 theScale = enemy.transform.localScale;
		theScale.z *= -1;
		theScale.z *= 1;
		transform.localScale = theScale;
		finished = true;
	}



	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			Debug.Log ("Player entered the view field");
			spotted = true;
			PlayerPos = other.transform;
		} else {
			spotted = false;
		}
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player") {
			spotted = true;
			PlayerPos = other.transform;
			agent.destination = PlayerPos.position;
			Debug.Log ("Player is still there");

		} else {
			spotted = false;
		}
	}

//	void OnTriggerStay(Collider other) {
//		if (other.tag == "Player") {
//			direction = other.transform.position + new Vector3(0,3,0) - eyePosition;
//			if (facingRight) {
//				angle = Vector3.Angle (direction, Vector3.right);
//				direction = Vector3.right;
//			} else {
//				angle = Vector3.Angle (direction, Vector3.left);
//				direction = Vector3.left;
//			}
//			// Debug.Log ("Angle " + angle + " condeDeg " + coneDegrees);
//
//			if (angle <= coneDegrees) {
//				Debug.Log ("Drawing Line!");
//				Debug.DrawRay(eyePosition,direction*1000f, Color.red, duration:5f);
//				if (Physics.Raycast (eyePosition, direction, out hitInfo)) {
//					Debug.Log (hitInfo.collider.tag);
//					if (hitInfo.collider.tag == "Player") {
//						Debug.Log ("Hit by Player");
//						Debug.DrawRay(eyePosition, direction, Color.red);
//						spotted = true;
//						PlayerPos = other.transform;
//					} else {
//						Debug.Log ("Did not hit the Player but hit " + hitInfo.collider.name);
//						Debug.DrawRay(eyePosition, direction, Color.green);
//						spotted = false;
//					}
//				} else {
//					Debug.DrawRay(eyePosition, direction, Color.green);
//					spotted = false;
//				}
//			} else {
//				Debug.DrawRay(eyePosition, direction, Color.green);
//				spotted = false;
//			}
//		}
//	}

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
