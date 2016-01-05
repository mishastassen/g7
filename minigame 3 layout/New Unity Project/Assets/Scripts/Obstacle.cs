using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
    public float movementspeed;

	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, movementspeed);
	}
}
