using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PreSceneUI : MonoBehaviour {

	public string levelName;

	public void OnStartButtonClick(){
		Eventmanager.Instance.triggerLevelFinished(levelName);
	}

	public void OnLocalMultiplayerButtonClick(){
		if(NetworkServer.active){
			Gamemanager.Instance.localmultiplayer = true;
			Gamemanager.Instance.onNextLevelLoad += addPlayer;
		}
		Eventmanager.Instance.triggerLevelFinished(levelName);
	}

	void addPlayer(){
		ClientScene.AddPlayer(2);
	}
}
