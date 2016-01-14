using UnityEngine;
using System.Collections;

public class PlayerAudioManager : MonoBehaviour {

	public AudioClip clipGetHurt;
	public AudioClip clipFootstepWood1;
	public AudioClip clipFootstepWood2;
	public AudioClip clipFailSound;
	public AudioClip clipSuccesSound;

	[HideInInspector]
	public AudioSource audioGetHurt, audioFootstepWood1, audioFootstepWood2, failSound, succesSound;

	void Awake(){
		audioGetHurt = AddAudio (clipGetHurt, false, false, 0.2f,100);
		audioFootstepWood1 = AddAudio (clipFootstepWood1, false, false, 0.1f,160);
		audioFootstepWood2 = AddAudio (clipFootstepWood2, false, false, 0.1f,160);
		failSound = AddAudio (clipFailSound, false, false, 1, 100);
		succesSound = AddAudio (clipSuccesSound, false, false, 1, 100);
	}

	public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol, byte priority) { 
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip; 
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol; 
		newAudio.priority = priority;
		return newAudio; 
	}
}
