using UnityEngine;
using System.Collections;

public class DoorMinigame : MonoBehaviour {

	private Rigidbody door;
	private Animator anim;
	private BoxCollider box;
	public bool doorOpen;
	private float openedCenterY = 3.2f;
	private float openedSizeY = 0.4f;
	private float closedCenterY = 1.75f;
	private float closedSizeY = 3.5f;

	void Start () {
		door = GetComponent<Rigidbody>();
		anim = GetComponent<Animator> ();
		box = GetComponent<BoxCollider> ();
	}

	void Update () {
		if (doorOpen) {
			anim.SetBool ("isOpen", true);
			Vector3 center = box.center;
			center.y=openedCenterY;
			box.center = center;
			Vector3 size = box.size;
			size.y=openedSizeY;
			box.size = size;
		} else {
			anim.SetBool ("isOpen", false);
			Vector3 center = box.center;
			center.y=closedCenterY;
			box.center = center;
			Vector3 size = box.size;
			size.y=closedSizeY;
			box.size = size;
		}
	}
}
