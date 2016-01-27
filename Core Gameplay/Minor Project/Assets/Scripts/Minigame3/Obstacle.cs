using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    public float movementspeed;

    // Update is called once per frame
    void Update()
    {
		float movement = movementspeed * Time.deltaTime;
        transform.Translate(0, 0, movement);
    }
}