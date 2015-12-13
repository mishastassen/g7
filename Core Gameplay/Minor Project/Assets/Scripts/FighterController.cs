using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FighterController : NetworkBehaviour {
	
	private float patrolSpeed;
	private bool isGroundedLeft, isGroundedRight;

	[SyncVar(hook="OnFacingChange")]
	public bool facingRight;	
	[SyncVar(hook="OnAnimationChange")]
	private bool isRunning;
	[SyncVar(hook="OnJumpingChange")]
	private bool isJumping;

	private Rigidbody enemy;
	private Animator anim;

	private enum FighterState {
		Patrolling,
		Fighting
	}

	private FighterState curState;


	void Start () {
		patrolSpeed = 4;
		facingRight = true;
		enemy = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator> ();
		curState = FighterState.Patrolling;
	}

	void Update() {
		if (!NetworkServer.active)
			return;
		isGroundedLeft = CheckGroundedLeft ();
		isGroundedRight = CheckGroundedRight ();
		if (curState == FighterState.Patrolling)
			UpdatePatrolling ();
		else if (curState == FighterState.Fighting)
			UpdateFighting ();
	}
	
	void FixedUpdate() {
	}

	void UpdatePatrolling() {
		Vector3 curSpeed = enemy.velocity;
		bool isWalkingRight = curSpeed.x>0;
		if ((isWalkingRight && !isGroundedRight) || 
			(!isWalkingRight && !isGroundedLeft)) {
			flip ();
			facingRight = !facingRight;
		}
		curSpeed.x = facingRight ? patrolSpeed : -patrolSpeed;
		enemy.velocity = curSpeed;
		isRunning = true;
	}

	void UpdateFighting(){
	}
	
	// checks whether the right of the enemy is on a platform
	bool CheckGroundedRight() {
		Vector3 rightPosition = new Vector3 (enemy.transform.position.x + 0.5f, enemy.transform.position.y + 1, enemy.transform.position.z);
		return Physics.Raycast (rightPosition, Vector3.down, 2);
	}
	
	// checks whether the left of the enemy is on a platform
	bool CheckGroundedLeft() {
		Vector3 leftPosition= new Vector3 (enemy.transform.position.x - 0.5f, enemy.transform.position.y + 1, enemy.transform.position.z);
		return Physics.Raycast (leftPosition, Vector3.down, 2);
	}	
	
	void flip () {
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	void OnFacingChange(bool facingRight) {
		Vector3 theScale = transform.localScale;
		theScale.x = facingRight ? 1 : -1;
		transform.localScale = theScale;
	}
	
	void OnAnimationChange(bool isRunning) {
		anim.SetBool ("isRunning", isRunning);
	}
	
	void OnJumpingChange(bool isJumping) {
		anim.SetBool ("isJumping", isJumping);
	}

}
