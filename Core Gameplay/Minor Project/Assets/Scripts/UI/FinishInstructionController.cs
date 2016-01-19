using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishInstructionController : MonoBehaviour {

	private int playerCount;
	private bool finished;

	public Text finishInstruction;
	public Image instructionPlank;

	void Start () {
		playerCount = 0;
		finished = false;
		finishInstruction.enabled = false;
		instructionPlank.enabled = false;
	}
	
	void Update () {
		if (playerCount > 0 && !finished) {
			finishInstruction.enabled = true;
			instructionPlank.enabled = true;
		} else { 
			finishInstruction.enabled = false;
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
		if (other.tag == "Package1" && other.transform.position.x > 90) {
			finished = true;
		}
	}
}
