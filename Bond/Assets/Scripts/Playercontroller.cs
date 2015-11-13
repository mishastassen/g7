using UnityEngine;
using System.Collections;

public class Playercontroller : MonoBehaviour {

	public float speedFactor;


	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool IsGrounded() {
		return Mathf.Abs (rb.velocity.y) < 0.1;
	}

	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, 0.0f);
		//rb.AddForce (speedFactor*movement);
		float yVelocity = rb.velocity.y;
		if(Input.GetButtonDown("Jump") && IsGrounded())
			yVelocity = 10.0f;
		rb.velocity = new Vector3(speedFactor*moveHorizontal, yVelocity, 0.0f);
	}
}
