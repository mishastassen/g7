using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThirdInstructionController : MonoBehaviour {

	public Text thirdInstruction;
	public Image instructionPlank;

	void Start () {
		thirdInstruction.enabled = false;
		instructionPlank.enabled = false;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			thirdInstruction.enabled = true;
			instructionPlank.enabled = true;
		}
	}

}