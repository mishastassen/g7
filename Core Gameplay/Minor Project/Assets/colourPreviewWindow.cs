using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class colourPreviewWindow : MonoBehaviour {

	byte r;
	byte g;
	byte b;

	public Slider redSlider;
	public Slider greenSlider;
	public Slider blueSlider;

	void Update () {
		r = (byte)redSlider.value;
		g = (byte)greenSlider.value;
		b = (byte)blueSlider.value;

		gameObject.GetComponent<RawImage> ().color = new Color32 (r, g, b, 255);
	}
}
