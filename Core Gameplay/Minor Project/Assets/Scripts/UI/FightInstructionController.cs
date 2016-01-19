using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FightInstructionController : MonoBehaviour {

	private int playerCount;

	public Text fightInstruction;
	public Image instructionPlank;

	void Start () {
		playerCount = 0;
		fightInstruction.enabled = false;
		instructionPlank.enabled = false;
	}
	
	void Update () {
		if (playerCount > 0) {
			fightInstruction.enabled = true;
			instructionPlank.enabled = true;
		} else {
			fightInstruction.enabled = false;
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
	}
}
