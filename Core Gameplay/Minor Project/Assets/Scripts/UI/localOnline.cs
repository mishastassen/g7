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
		localOnlineCanvas.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", false);
		levelSelectCanvas.transform.FindChild ("BackButtonContainer").GetComponent<Animator> ().SetBool ("Enabled", true);
	}

	public void pressOffline(){
		WebManager.Instance.localmultiplayer = true;
		NetworkManager.singleton.StartHost ();
	}

	void Update(){
		if (gameObject.GetComponent<Canvas> ().enabled && WebManager.Instance.currentUser != null) {
			WebManager.Instance.localmultiplayer = true;
			NetworkManager.singleton.StartHost ();
		}
	}

}
