using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class connectButton : MonoBehaviour {

	public User linkedUser;

	public NetworkManager networkmanager;
	public WebManager webmanager;

	public void onClick(){
		JSONClass messageBody = new JSONClass();
		messageBody ["reqUserId"].AsInt = 0;
		StartCoroutine (webmanager.IEsendMessage (1,"playGame",messageBody));
	}
}
