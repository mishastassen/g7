using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class minigame3Explainer: MonoBehaviour {

	private int playerCount;

	public GameObject minigameInstruction;

	void Start () {
		playerCount = 0;
	}

	void Update () {
		if (playerCount == 2) {
			minigameInstruction.SetActive (true);
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
