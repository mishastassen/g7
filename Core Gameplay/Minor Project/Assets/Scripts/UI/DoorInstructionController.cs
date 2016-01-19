using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorInstructionController : MonoBehaviour {

	private int playerCount;
	private bool doorOpen;
	private bool eventEnabled;

	public Text doorInstruction;
	public Image instructionPlank;

	void Start () {
		playerCount = 0;
		doorOpen = false;
		doorInstruction.enabled = false;
		instructionPlank.enabled = false;
		Eventmanager.Instance.EventonSwitchPulled += SwitchPulled;
		eventEnabled = true;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
	}
	
	void Update () {
		if (playerCount > 0 && !doorOpen) {
			doorInstruction.enabled = true;
			instructionPlank.enabled = true;
		}
	}

	void SwitchPulled (int id) {
		doorOpen = true;
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

	void OnDisable(){
		if (eventEnabled) {
			Eventmanager.Instance.EventonSwitchPulled -= SwitchPulled;
			eventEnabled = false;
		}
	}
}
