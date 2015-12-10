using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WebManager : MonoBehaviour {

	public string server = "http://drproject.twi.tudelft.nl:8088";
	string cookie = "{ sessionId = 1}";

	public void login(){
		JSONClass JSON = new JSONClass();
		JSON.Add ("username", new JSONData ("admin"));
		JSON.Add ("password", new JSONData ("root"));
		WWW www = createJSON (JSON.ToString (), "/login");
		StartCoroutine(getWWW (www));
	}

	public void logout(){
		WWW www = new WWW (server + "/logout");
		StartCoroutine(getWWW (www));
	}

	public void response(){
		WWW www = new WWW (server + "/response");
		StartCoroutine(getWWW (www));
	}

	IEnumerator getWWW(WWW www){
		Debug.Log ("getting url");
		yield return www;
		Debug.Log (www.text);
		cookie = www.responseHeaders ["Set-Cookie"];
	}

	WWW createJSON(string JSONdata, string path){
		
		Dictionary<string,string> headers = new Dictionary<string,string>();
		headers.Add("Content-Type", "application/json");
		headers.Add ("Cookie", cookie);
		
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(JSONdata.ToCharArray());
		
		return new WWW(server + path, pData, headers);
	}
}
