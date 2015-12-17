using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class loggedInText : MonoBehaviour {

	void Update() {
		if (this.gameObject.activeInHierarchy && WebManager.Instance.currentUser!= null) {
			this.gameObject.GetComponent<Text> ().text = "Logged in as: " + WebManager.Instance.currentUser.Username;
		}
	}

}
