using UnityEngine;
using System.Collections;

public class WebManager : MonoBehaviour {

	void Start(){
		StartCoroutine (getUrl ());
	}

	IEnumerator getUrl(){
		Debug.Log ("getting url");
		WWW www = new WWW ("http://drproject.twi.tudelft.nl:8088");
		yield return www;
		Debug.Log (www.text);
	}

}
