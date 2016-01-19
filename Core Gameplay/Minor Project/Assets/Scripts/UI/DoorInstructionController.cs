using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorInstructionController : MonoBehaviour {

	private int playerCount;
	private bool throughDoor;
	private bool eventEnabled;

	public Text doorInstruction;
	public Image instructionPlank;

	void Start () {
		playerCount = 0;
		throughDoor = false;
		doorInstruction.enabled = false;
		instructionPlank.enabled = false;
	}
	
	void Update () {
		if (playerCount > 0 && !throughDoor) {
			doorInstruction.enabled = true;
			instructionPlank.enabled = true;
		} else {
			doorInstruction.enabled = false;
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
		if (other.tag == "Player" && other.transform.position.x > 48) {
			throughDoor = true;
		}
	}
}
