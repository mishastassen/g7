using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumberScript : MonoBehaviour {

	EventScript eventscript;

	private int counter;

	// Use this for initialization
	void Start () {
		counter = 0;
	}

	public void startListener(){
		eventscript = (EventScript) GameObject.FindObjectOfType (typeof(EventScript));
		eventscript.EventOnSpaceEvent += count;
		Debug.Log ("Listener started");
	}

	void Update(){
		this.gameObject.GetComponent<Text> ().text = counter.ToString();
	}

	public void count () {
		Debug.Log ("Counter increased");
		counter++;
	}
}
