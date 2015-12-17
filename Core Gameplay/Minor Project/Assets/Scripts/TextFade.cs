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
		yield return new WaitForSeconds (6f);
		firstinstruction.enabled = false;
		yield return new WaitForSeconds (3f);
		secondinstruction.enabled = true;
		yield return new WaitForSeconds (6f);
		secondinstruction.enabled = false;
	}
}
