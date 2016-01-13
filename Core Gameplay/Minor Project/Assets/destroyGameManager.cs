using UnityEngine;
using System.Collections;

public class destroyGameManager : MonoBehaviour {

	void Awake(){
		GameObject gamemanager = GameObject.Find ("Game manager");;
		if (gamemanager != null) {
			gamemanager.GetComponent<GamemanagerEventHandler> ().stopManager ();
			gamemanager.GetComponent<Gamemanager> ().stopManager ();
			Destroy (gamemanager);
		}
	}
}
