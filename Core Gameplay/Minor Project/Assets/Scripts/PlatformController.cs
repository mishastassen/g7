using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlatformController : NetworkBehaviour {

	public PathDefinition path;
	public float speed=1;
	public float inRangeGoal = 0.1f;

	private IEnumerator<Transform> currentPoint;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		if (path == null) {
			Debug.LogError("path cannot be null: "+gameObject);
		}
		rb = GetComponent<Rigidbody>();
		currentPoint = path.pathEnumerator();
		currentPoint.MoveNext ();
		if (currentPoint.Current==null)
			return;

		transform.position = currentPoint.Current.position;
	}
	
	void FixedUpdate () {
		if (currentPoint == null || currentPoint.Current.position == null)
			return;
		transform.position = Vector3.MoveTowards (transform.position, currentPoint.Current.position, Time.deltaTime*speed);
		float distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
		if (distanceSquared < inRangeGoal * inRangeGoal)
			currentPoint.MoveNext ();
	}
}
