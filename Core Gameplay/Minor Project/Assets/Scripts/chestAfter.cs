using UnityEngine;
using System.Collections;

public class chestAfter : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		anim.SetBool ("isCompleted", true);
		Debug.Log ("moet nu openen");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
