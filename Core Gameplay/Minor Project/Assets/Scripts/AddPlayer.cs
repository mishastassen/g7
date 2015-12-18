using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AddPlayer : NetworkBehaviour {

	public void OnButtonClick(){
		if(NetworkServer.active){
			ClientScene.AddPlayer(2);
			Gamemanager.Instance.localmultiplayer = true;
		}
	}
}
