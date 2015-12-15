using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WebManager : MonoBehaviour {

	/*server info*/
	public string server = "http://drproject.twi.tudelft.nl:8088";
	string cookie = "";

	/*UI input*/
	public GameObject loginName, loginPass, createName, createPass, responseText, onlineUserText, friendsText;

	/*Users*/
	public User currentUser;
	public List<User> friendList = new List<User>();  //Friend list
	public List<User> requestList = new List<User> (); //Pending friend requests
	public List<User> onlineUsersList = new List<User>(); //Online Users
	
	public void login(){
		StartCoroutine(IElogin ());
	}

	public void logout(){
		StartCoroutine(IElogout ());
	}

	public void response(){
		StartCoroutine(IEresponse());
	}

	public void createAccount(){
		StartCoroutine(IEcreateAccount());
	}

	public void getFriendList(){
		StartCoroutine(IEgetFriendList ());
	}

	public void getUsers(){
		StartCoroutine(IEgetUsers ());
	}



	IEnumerator IElogin(){
		JSONClass JSON = new JSONClass();
		JSON.Add ("username", new JSONData (loginName.GetComponent<InputField>().text));
		JSON.Add ("password", new JSONData (loginPass.GetComponent<InputField>().text));
		WWW www = createJSON (JSON.ToString (), "/login");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		if (result == "Wrong username" || result == "Wrong password") {
			Debug.LogError("Incorrect login info");
			yield break;
		}
		currentUser = new User (SimpleJSON.JSON.Parse (result));
		StartCoroutine (update());
	}

	IEnumerator IElogout(){
		WWW www = createEmptyWWW ("/logout");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		currentUser = null;
	}

	IEnumerator IEresponse(){
		WWW www = createEmptyWWW ("/response");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
	}

	IEnumerator IEcreateAccount(){
		JSONClass JSON = new JSONClass();
		JSON.Add ("username", new JSONData (createName.GetComponent<InputField>().text));
		JSON.Add ("password", new JSONData (createPass.GetComponent<InputField>().text));
		WWW www = createJSON (JSON.ToString (), "/createAccount");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
	}

	IEnumerator IEgetFriendList(){
		WWW www = createEmptyWWW ("/updateFriends");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		if (result == "no need to update friendlist") {
			yield break;
		}
		JSONNode friendListJSON = SimpleJSON.JSON.Parse (result);
		int index = 0;
		friendList = new List<User> ();
		while (friendListJSON[index]["UserId"] != null) {
			User friend = new User(friendListJSON[index]);
			friendList.Add(friend);
			index++;
		}
		updateFriendsText (friendList);
	}

	IEnumerator IEgetFriendRequests(){
		WWW www = createEmptyWWW ("/updateFriendRequests");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		if (result == "no need to update friendrequests") {
			yield break;
		}
		JSONNode requestListJSON = SimpleJSON.JSON.Parse (result);
		int index = 0;
		requestList = new List<User> ();
		while (requestListJSON[index]["UserId"] != null) {
			User request = new User(requestListJSON[index]);
			requestList.Add(request);
			index++;
		}
	}

	IEnumerator IEgetUsers(){
		WWW www = createEmptyWWW ("/updateUsers");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		JSONNode userListJSON = SimpleJSON.JSON.Parse (result);
		onlineUsersList = new List<User> ();
		foreach( var key in userListJSON.Keys )
		{
			User user = new User(userListJSON[key]);
			onlineUsersList.Add(user);
		}
		updateUserText (onlineUsersList);
	}
	
	IEnumerator update(){
		while (currentUser != null) {
			StartCoroutine (IEgetUsers ());
			StartCoroutine (IEgetFriendList ());
			StartCoroutine (IEgetFriendRequests ());
			yield return new WaitForSeconds (4.0f);
		}
	}

	IEnumerator getWWW(WWW www){
		yield return www;

		if (!string.IsNullOrEmpty(www.error)){
			Debug.LogError("Server request failed");
			yield return null;
		}
		responseText.GetComponent<Text>().text = www.text;
		if(www.responseHeaders.ContainsKey("SET-COOKIE")){
			cookie = www.responseHeaders ["SET-COOKIE"];
		}
		if (www.text == "Log in first") {
			Debug.LogError ("Not logged in");
			currentUser = null;
			yield return null;
		} else {
			yield return www.text;
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
	
	WWW createEmptyWWW(string path){
		Dictionary<string,string> headers = new Dictionary<string,string>();
		if (cookie != "") {
			headers.Add ("cookie", cookie);
		}
		return new WWW(server + path, null, headers);
	}

	void updateUserText(List<User> users){
		Text usertext = onlineUserText.GetComponent<Text> ();
		string userString = "";
		foreach (User user in users) {
			userString += user.Username +" " + user.levelProgress +"\n";
		}
		usertext.text = userString;
	}

	void updateFriendsText(List<User> friends){
		Text usertext = friendsText.GetComponent<Text> ();
		string userString = "";
		foreach (User user in friends) {
			userString += user.Username +" " + user.levelProgress +" " + user.Online + "\n";
		}
		usertext.text = userString;
	}
}
