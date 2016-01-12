using UnityEngine;
using System.Collections;

public class PlayerAudioManager : MonoBehaviour {

	public AudioClip clipGetHurt;
	public AudioClip clipFootstepWood1;
	public AudioClip clipFootstepWood2;

	[HideInInspector]
	public AudioSource audioGetHurt, audioFootstepWood1, audioFootstepWood2;

	void Awake(){
		audioGetHurt = AddAudio (clipGetHurt, false, false, 0.8f);
		audioFootstepWood1 = AddAudio (clipFootstepWood1, false, false, 0.4f);
		audioFootstepWood2 = AddAudio (clipFootstepWood2, false, false, 0.4f);
	}

	public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) { 
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip; 
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol; 
		return newAudio; 
	}
}
