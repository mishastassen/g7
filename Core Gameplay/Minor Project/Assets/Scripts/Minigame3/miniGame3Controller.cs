using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class miniGame3Controller : NetworkBehaviour {

	public int lives;
	public Text livestext;
    public Image losePlank;
	public Text loseText;
	public Text loseInstruction;

	void Start(){
		SetLives ();
		losePlank.enabled = false;
		loseText.enabled = false;
		loseInstruction.enabled = false;
    }

	public void SetLives()
	{
		livestext.text = "Lives: " + lives.ToString();
	}

    void Update() { 
		if (lives<=0){
			losePlank.enabled = true;
			loseText.enabled = true;
			loseInstruction.enabled = true;

			if (isServer && Input.GetButtonDown ("Interact1_P1")) {
				Eventmanager.Instance.triggerLevelSwitch ("Minigame3");
			}
        }
    }
}
