using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class miniGame3Controller : NetworkBehaviour {

//	[SyncVar(hook="SetLives")]
	public int lives = 30;
	public Text livestext;

	void Start(){
		SetLives ();
	}

	public void SetLives()
	{
		livestext.text = "Lives: " + lives.ToString();
	}

}
