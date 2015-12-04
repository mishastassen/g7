using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PreSceneUI : MonoBehaviour {

	public string levelName;

	public void OnButtonClick(){
		Eventmanager.Instance.triggerLevelFinished(levelName);
	}
}
