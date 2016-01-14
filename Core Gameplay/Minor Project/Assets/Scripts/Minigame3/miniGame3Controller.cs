using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class miniGame3Controller : NetworkBehaviour {

	public int lives;
	public Text livestext;
    public Text lose;

	void Start(){
		SetLives ();
        lose.text = "";
    }

	public void SetLives()
	{
		livestext.text = "Lives: " + lives.ToString();
	}

    void Update() { 
    if (lives==0){
            lose.text = "You lose";
        }
    }
}
