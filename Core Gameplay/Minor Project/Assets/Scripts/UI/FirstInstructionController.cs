using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstInstructionController : MonoBehaviour {

	private int playerCount;

	public Text firstInstruction;
	public Image instructionPlank;

	void Start () {
		playerCount = 2;
		firstInstruction.enabled = true;
		instructionPlank.enabled = true;
	}

	void Update () {
		if (playerCount == 0) {
			firstInstruction.enabled = false;
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
