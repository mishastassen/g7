using UnityEngine;
using System.Collections;

public class levelFinishScreen : MonoBehaviour {

	public string nextLevel;

	public void nextLevelButton(){
		Eventmanager.Instance.triggerLevelSwitch (nextLevel);
	}
}
