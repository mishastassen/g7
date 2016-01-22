using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class level6Explainer : MonoBehaviour {
	
	private int playerCount;

	public GameObject levelInstruction;

	void Start () {
		playerCount = 0;
	}

	void Update () {
		if (playerCount == 2) {
			levelInstruction.SetActive (true);
		} else {
			levelInstruction.SetActive (false);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			if (playerCount != 2) {
				playerCount += 1;
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			playerCount -= 1;
		}
	}
}
