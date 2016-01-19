using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AddPlayer : NetworkBehaviour {

	public void OnButtonClick(){
		ClientScene.AddPlayer (NetworkManager.singleton.client.connection, 0);
	}
}
