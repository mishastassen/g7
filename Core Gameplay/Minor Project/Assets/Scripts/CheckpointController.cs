using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {

	private Animator anim;

	public int checkpointNum;
	public Transform playerSpawn;

	bool eventsEnabled;

	void OnEnable(){
		Eventmanager.Instance.EventonCheckpointReached += HandleEventonCheckpointReached;
		Gamemanager.Instance.onDisableEventHandlers += OnDisable;
		eventsEnabled = true;
		anim = this.GetComponent<Animator> ();
		anim.speed = 1.5f;
	}

	void OnDisable(){
		if (eventsEnabled) {
			Eventmanager.Instance.EventonCheckpointReached -= HandleEventonCheckpointReached;
			eventsEnabled = false;
		}
	}

	void HandleEventonCheckpointReached (int checkpointNum)
	{
		if (checkpointNum == this.checkpointNum) {
			// gameObject.GetComponent<Renderer>().material.color = Color.red;
			anim.SetBool("reached",true);
		}
	}
	
}
