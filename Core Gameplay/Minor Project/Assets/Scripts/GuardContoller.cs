using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GuardContoller : NetworkBehaviour {

	private Rigidbody enemy;
	private Vector3 rayStart;

	private bool finished;
	private bool facingRight;
	private float spottingDistance;
	private RaycastHit hitInfo;

	void Start () {
		enemy = GetComponent<Rigidbody> ();
		rayStart = new Vector3 (enemy.position.x, enemy.position.y + 5.0f, enemy.position.z);
		finished = true;
		facingRight = true;
		spottingDistance = 40.0f;
	}
	
	void FixedUpdate () {
		if (finished) {
			StartCoroutine (flipGuard ());
			finished = false;
			facingRight = !facingRight;
		}
		if (facingRight) {
			Debug.DrawRay (rayStart, Vector3.right, Color.red);
			Physics.Raycast(rayStart, Vector3.right, out hitInfo);
			if (hitInfo.collider.tag == "Player" && hitInfo.distance <= spottingDistance) {
				CmdPlayerSpotted();
			} 
		} else {
			Debug.DrawRay (rayStart, Vector3.left, Color.blue);
			Physics.Raycast(rayStart, spottingDistance * Vector3.left, out hitInfo);
			if (hitInfo.collider.tag == "Player" && hitInfo.distance <= spottingDistance) {
				CmdPlayerSpotted();
			} 
		}
	}

	IEnumerator flipGuard () {
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
		yield return new WaitForSeconds(5);
		finished = true;
	}

	[Command]
	void CmdPlayerSpotted() {
		Eventmanager.Instance.triggerPlayerSpotted();
	}
}
