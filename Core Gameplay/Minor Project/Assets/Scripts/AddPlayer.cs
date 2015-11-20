using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AddPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonClick(){
		if(NetworkServer.active){
			if(ClientScene.localPlayers.Count == 1){
				ClientScene.AddPlayer(2);
			}
		}
	}
}
