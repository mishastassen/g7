using UnityEngine;
using System.Collections;

public class openEscMenu : MonoBehaviour {
	
	public GameObject pauseMenu;

	public void triggerEscMenu(){
		if (pauseMenu.activeSelf) {
			pauseMenu.SetActive (false);
		} else {
			pauseMenu.SetActive (true);
		}
	}
}
