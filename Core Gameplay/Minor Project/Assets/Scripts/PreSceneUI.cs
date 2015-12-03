using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PreSceneUI : MonoBehaviour {
	public void OnButtonClick(){
		NetworkManager Manager = GameObject.Find ("Network manager").GetComponent<NetworkManager>();
		Manager.ServerChangeScene ("BasisLevel");
	}
}
