using UnityEngine;
using System.Collections;

public class Scrollingtexture : MonoBehaviour {

    public float speed;

	void Update () {
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, Time.time * speed);
	}
}
