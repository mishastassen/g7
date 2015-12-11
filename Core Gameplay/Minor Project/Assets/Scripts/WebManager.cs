using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WebManager : MonoBehaviour {

	public string server = "http://drproject.twi.tudelft.nl:8088";
	string cookie = "";

	public GameObject loginName, loginPass, createName, createPass, responseText;

	public void login(){
		JSONClass JSON = new JSONClass();
		JSON.Add ("username", new JSONData (loginName.GetComponent<InputField>().text));
		JSON.Add ("password", new JSONData (loginPass.GetComponent<InputField>().text));
		WWW www = createJSON (JSON.ToString (), "/login");
		StartCoroutine(getWWW (www));
	}

	public void logout(){
		WWW www = createEmpty ("/logout");
		StartCoroutine(getWWW (www));
	}

	public void response(){
		WWW www = createEmpty ("/response");
		StartCoroutine(getWWW (www));
	}

	public void createAccount(){
		JSONClass JSON = new JSONClass();
		JSON.Add ("username", new JSONData (createName.GetComponent<InputField>().text));
		JSON.Add ("password", new JSONData (createPass.GetComponent<InputField>().text));
		WWW www = createJSON (JSON.ToString (), "/createAccount");
		StartCoroutine(getWWW (www));
	}

	IEnumerator getWWW(WWW www){
		yield return www;
		responseText.GetComponent<Text> ().text = www.text;

		if(www.responseHeaders.ContainsKey("SET-COOKIE")){
			cookie = www.responseHeaders ["SET-COOKIE"];
		}
	}

	WWW createJSON(string JSONdata, string path){
		
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		if (cookie != "") {
			headers.Add ("cookie", cookie);
		}
		
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(JSONdata.ToCharArray());
		
		return new WWW(server + path, pData, headers);
	}

	WWW createEmpty(string path){
		Dictionary<string,string> headers = new Dictionary<string,string>();
		if (cookie != "") {
			headers.Add ("cookie", cookie);
		}
		return new WWW(server + path, null, headers);
	}
}
