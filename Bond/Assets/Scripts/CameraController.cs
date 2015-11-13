using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject Player;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = this.transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate() {
		this.transform.position = Player.transform.position + offset;
	}
}
