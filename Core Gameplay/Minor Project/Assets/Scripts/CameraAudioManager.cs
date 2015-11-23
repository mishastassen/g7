using UnityEngine;
using System.Collections;

public class CameraAudioManager : MonoBehaviour {

	public AudioClip clipAmbient;

	private AudioSource audioAmbient;

	void Awake(){
		audioAmbient = AddAudio (clipAmbient, true, true, 0.1f);
		audioAmbient.Play ();
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
