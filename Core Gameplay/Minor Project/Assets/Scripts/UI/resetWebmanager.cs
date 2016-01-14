using UnityEngine;
using System.Collections;

public class resetWebmanager : MonoBehaviour {
	
	public GameObject responseText, popUpPanel;
	public GameObject loginResponse, createAccountResponse;
	public Canvas levelSelect, loggedIn;

	void Start () {
		WebManager.Instance.responseText = responseText;
		WebManager.Instance.popUpPanel = popUpPanel;
		WebManager.Instance.loginResponse = loginResponse;
		WebManager.Instance.createAccountResponse = createAccountResponse;
		WebManager.Instance.levelSelect = levelSelect;
		WebManager.Instance.loggedIn = loggedIn;
		WebManager.Instance.localmultiplayer = false;
		WebManager.Instance.otherPlayer = null;
	}

}
