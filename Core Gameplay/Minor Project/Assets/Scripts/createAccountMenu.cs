using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class createAccountMenu : MonoBehaviour {
	public Canvas networkMultiplayer;
	public Canvas accountCreate;

	public InputField createName;
	public InputField createPassword;

	public Slider redSlider;
	public Slider greenSlider;
	public Slider blueSlider;

	public void pressBack(){
		networkMultiplayer.enabled = true;
		accountCreate.enabled = false;
	}

	public void pressCreateAccount(){
		string hexColor = rgbToHex ((byte)redSlider.value,(byte)greenSlider.value,(byte)blueSlider.value);
		WebManager.Instance.createAccount (createName.GetComponent<InputField>().text,createPassword.GetComponent<InputField>().text,hexColor);
	}

	string rgbToHex(byte r, byte g, byte b){
		string chars = "0123456789ABCDEF";
		char[] alpha = chars.ToCharArray ();
		string hex = "#" + alpha[(r / 16)];
		hex += alpha [(r % 16)];
		hex += alpha [(g / 16)];
		hex += alpha [(g % 16)];
		hex += alpha [(b / 16)];
		hex += alpha [(b % 16)];
		return hex;
	}
}