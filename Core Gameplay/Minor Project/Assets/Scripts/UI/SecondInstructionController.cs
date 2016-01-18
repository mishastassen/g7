using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SecondInstructionController : MonoBehaviour {

	private int playerCount;
	private bool packageLeft;

	public Text secondInstruction;
	public Image instructionPlank;

	void Start () {
		playerCount = 0;
		packageLeft = false;
		secondInstruction.enabled = false;
	}

	void Update () {
		if (playerCount == 2 && !packageLeft) {
			secondInstruction.enabled = true;
		} else if (packageLeft) {
			secondInstruction.enabled = false;
			instructionPlank.enabled = false;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			playerCount += 1;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			playerCount -= 1;
		}
		if (other.tag == "Package1" && other.transform.position.x > -80) {
			packageLeft = true;
		}
	}
}
