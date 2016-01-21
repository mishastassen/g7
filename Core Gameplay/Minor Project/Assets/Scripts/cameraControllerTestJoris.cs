using UnityEngine;
using System.Collections;

public class cameraControllerTestJoris : MonoBehaviour {

	public GameObject left;
	public GameObject right;

	private Vector3 leftPos;
	private Vector3 rightPos;

	private float lerpTime;
	float currentLerpTime;

	// Use this for initialization
	void Start () {
		lerpTime = 5f;
		leftPos = left.transform.position;
		rightPos = right.transform.position;
		
		leftPos.z = -200f;
		rightPos.z = -200f;

		Debug.Log (leftPos);
		Debug.Log (rightPos);

	
	}
	
	// Update is called once per frame
	void Update () {
		currentLerpTime += Time.deltaTime;

		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}
		float perc = currentLerpTime / lerpTime;
		this.GetComponent<Transform> ().position = Vector3.Lerp (leftPos, rightPos, perc);

		Debug.Log (this.transform.position);
	}
}
