using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioController : MonoBehaviour {

	public Slider musicSlider;
	private GameObject webmanager;

	void Start () {
		webmanager = GameObject.Find ("Webmanager");
	}

	void Update () {
		webmanager.GetComponent<AudioSource> ().volume = musicSlider.value;
	}
}
