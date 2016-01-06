using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFade : MonoBehaviour {

	public Text firstinstruction;
	public Text secondinstruction;

	// Use this for initialization
	void Start () {
		secondinstruction.enabled = false;
		StartCoroutine (firstTextFade ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator firstTextFade(){
		yield return new WaitForSeconds (12f);
		firstinstruction.enabled = false;
		yield return new WaitForSeconds (5f);
		secondinstruction.enabled = true;
		yield return new WaitForSeconds (12f);
		secondinstruction.enabled = false;
	}
}
