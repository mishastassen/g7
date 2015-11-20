using UnityEngine;
using System.Collections;

// class for developing purposes
// makes things slower, so it's easier to judge animations etc.
// Seems to work in multiplayer by just adding to the Hierarchy
public class Slowmotion : MonoBehaviour {

	// Use this for initialization
	void Start () {	
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
