using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class localOnline : MonoBehaviour {
	public Canvas levelSelectCanvas;
	public Canvas localOnlineCanvas;

	public InputField loginName;
	public InputField loginPass;

	public void pressLogin(){
		WebManager.Instance.localmultiplayer = true;
		WebManager.Instance.login (loginName.text,loginPass.text);
		NetworkManager.singleton.StartHost ();
	}
	
	public void pressBack(){
		levelSelectCanvas.enabled = true;
		localOnlineCanvas.enabled = false;
	}

	public void pressOffline(){
		WebManager.Instance.localmultiplayer = true;
		NetworkManager.singleton.StartHost ();
	}

}
