using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject cam;

	public GameObject left;
	public GameObject right;

	private Vector3 leftPos;
	private Vector3 rightPos;
	private Vector3 camPos;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// left = GameObject.Find ("LeftPlayer");
		// right = GameObject.Find ("RightPlayer");

		leftPos = left.transform.position;
		rightPos = right.transform.position;

		float loc_x = (leftPos.x + rightPos.x) / 2.0f;
		float loc_y = 7.5f;
		float loc_z = ((leftPos.z + rightPos.z) / 2.0f) - 10.5f;

		camPos.Set(loc_x,loc_y,loc_z);

		cam.transform.position = camPos;
	
	}
}
