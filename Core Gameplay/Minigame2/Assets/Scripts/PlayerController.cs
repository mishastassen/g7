using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Rigidbody rb;

	public float speed;
	public float windspeed;
	public int rotatespeed;

	private bool windBlowing;
	private bool windBlowLong;

	int frame_count;
	int count;

	// Use this for initialization
	void Start () {
		count = 0;
		rb.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		frame_count++;
		windBlowing = false;
		if (frame_count % 200 == 0) {
			windBlowing = true;
		}

		if (windBlowing) {
			windBlowLong = true;
		}

		if (windBlowLong){
			//var windway = Random(-1,1);
			count ++;
			rb.AddForce(Vector3.left * windspeed);
			Debug.Log ( "Adding wind");
			

			if (count > 60){
				windBlowLong = false;
				count = 0;
			}
		}
	}

	void FixedUpdate()
	{
		
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			
		}
		
		
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Translate (-Vector3.forward * speed * Time.deltaTime);
			
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			rb.AddForce(Vector3.left * rotatespeed);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			rb.AddForce(-Vector3.left * rotatespeed);
		}
	}


}
