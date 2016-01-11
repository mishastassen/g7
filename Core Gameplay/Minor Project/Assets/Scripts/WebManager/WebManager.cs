using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class WebManager : MonoBehaviour {

	/*server info*/
	public string server = "http://drproject.twi.tudelft.nl:8088";
	string cookie = "";

	/*UI input*/
	public GameObject loginName, loginPass, responseText, onlineUserPrefab, onlineUserPanel, friendsText, popUpPanel, inputButtonPanel;

	/*UI response*/
	public GameObject loginResponse, createAccountResponse;

	/*Setup levels*/
	[HideInInspector]
	public bool localmultiplayer = false;
	public string level1;
	
	/*Users*/
	public User currentUser = null;
	public User otherPlayer = null;
	public List<User> friendList = new List<User>();  //Friend list
	public List<User> requestList = new List<User> (); //Pending friend requests
	public List<User> onlineUsersList = new List<User>(); //Online Users

	/*Make this Don't Destroy on Load and set instance*/
	private static WebManager static_instance = null;
	private static object _lock = new object ();
	private static bool applicationIsQuitting = false;

	public static WebManager Instance{
		get{
			if (applicationIsQuitting) {
				return null;
			}
			lock(_lock)
			{
				static_instance = (WebManager) FindObjectOfType(typeof(WebManager));

				if ( FindObjectsOfType(typeof(WebManager)).Length > 1 )
				{
					Debug.LogError("Warning: multiple WebManagers");
					return static_instance;
				}
				if(static_instance == null){
					Debug.LogError("Error no WebManager singleton");
				}
			}
			return static_instance;
		}
	}

	void Awake () {
		if (FindObjectsOfType (typeof(WebManager)).Length > 1) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad (gameObject);
		}
	}


	/*UI functions to call*/
	public void login(){
		StartCoroutine(IElogin (loginName.GetComponent<InputField>().text,loginPass.GetComponent<InputField>().text));
	}

	public void logout(){
		StartCoroutine(IElogout ());
	}

	public void response(){
		StartCoroutine(IEresponse());
	}

	public void createAccount(string username, string password, string hexColor){
		StartCoroutine(IEcreateAccount(username,password,"M",hexColor));
	}

	public void getFriendList(){
		StartCoroutine(IEgetFriendList ());
	}

	public void getUsers(){
		StartCoroutine(IEgetUsers ());
	}

	public void getHighscores(int levelId, levelFinishScreen endscreen){
		StartCoroutine (IEgetHighscores (levelId, endscreen));
	}

	public void updateHighscores(int levelId, int highscore){
		Debug.Log ("Updating highscores");
		StartCoroutine (IEupdateHighscores (levelId, highscore));
	}

	/*IEnumerators for coroutines*/
	IEnumerator IElogin(string username, string password){
		JSONClass JSON = new JSONClass();
		JSON.Add ("username", new JSONData (username));
		JSON.Add ("password", new JSONData (password));
		WWW www = createJSON (JSON.ToString (), "/login");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		if (result == "Wrong username" || result == "Wrong password") {
			if (loginResponse.activeInHierarchy) {
				loginResponse.GetComponent<Text> ().text = "Incorrect login info";
			}
			yield break;
		}
		if (loginResponse.activeInHierarchy) {
			loginResponse.GetComponent<Text> ().text = "";
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

	IEnumerator IEcreateAccount(string username, string password, string sex, string color){
		JSONClass JSON = new JSONClass();
		JSON.Add ("username", new JSONData (username));
		JSON.Add ("password", new JSONData (password));
		JSON.Add ("sex", new JSONData (sex));
		JSON.Add ("color", new JSONData (color));
		WWW www = createJSON (JSON.ToString (), "/createAccount");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		if (createAccountResponse.activeInHierarchy) {
			createAccountResponse.GetComponent<Text> ().text = result;
		}
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
		//updateFriendsText (friendList);
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
		if (SceneManager.GetActiveScene().buildIndex == 0) {
			updateUserText (onlineUsersList);
		}
	}

	IEnumerator IEgetMessages(){
		WWW www = createEmptyWWW ("/getMessages");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		if (result == "No messages") {
			yield break;
		}
		JSONNode message = SimpleJSON.JSON.Parse (result);
		messageReceived (message);
	}

	public IEnumerator IEsendMessage(int receipId,string type,JSONClass messageBody){
		JSONClass message = new JSONClass();
		message["ReceipId"].AsInt = receipId;
		message["Type"] = type;
		message["messageBody"] = messageBody;
		WWW www = createJSON (message.ToString (), "/sendMessage");
		CoroutineWithData cd = new CoroutineWithData(this,getWWW(www));
		yield return cd.coroutine;
		string result = (string)cd.result;
	}

	IEnumerator IEgetHighscores(int levelId, levelFinishScreen endscreen){
		JSONClass message = new JSONClass ();
		message ["LevelId"].AsInt = levelId;
		message ["UserId"].AsInt = currentUser.UserId;
		WWW www = createJSON (message.ToString (), "/getHighscores");
		CoroutineWithData cd = new CoroutineWithData (this, getWWW (www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		JSONNode highscores = SimpleJSON.JSON.Parse(result);
		endscreen.displayHighscores (highscores);
	}

	IEnumerator IEupdateHighscores(int levelId, int highscore){
		JSONClass message = new JSONClass ();
		message ["LevelId"].AsInt = levelId;
		message ["UserId"].AsInt = currentUser.UserId;
		if (otherPlayer != null) {
			message ["Player2Id"].AsInt = otherPlayer.UserId;
		} else {
			message ["Player2Id"].AsInt = 0;
		}
		message ["Highscore"].AsInt = highscore;
		WWW www = createJSON (message.ToString (), "/updateHighscores");
		CoroutineWithData cd = new CoroutineWithData (this, getWWW (www));
		yield return cd.coroutine;
		string result = (string)cd.result;
		if (result != "Succes") {
			Debug.LogError ("Problem updating highscores");
		}
	}

	IEnumerator update(){
		while (currentUser != null) {
			StartCoroutine (IEgetUsers ());
			StartCoroutine (IEgetFriendList ());
			StartCoroutine (IEgetFriendRequests ());
			StartCoroutine (IEgetMessages ());
			yield return new WaitForSeconds (2.0f);
		}
	}

	IEnumerator getWWW(WWW www){
		yield return www;

		if (!string.IsNullOrEmpty(www.error)){
			Debug.LogError("Server request failed");
			currentUser = null;
			yield return null;
		}
		//responseText.GetComponent<Text>().text = www.text;
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


	/*Constructors for different WWW objects*/
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


	/*Functions called in script*/
	void updateUserText(List<User> users){
		if (onlineUserPanel.activeInHierarchy) {
			List<GameObject> oldText = new List<GameObject> ();
			foreach (Transform child in onlineUserPanel.transform)
				oldText.Add (child.gameObject);
			oldText.ForEach (child => Destroy (child));
			foreach (User user in users) {
				GameObject text = Instantiate (onlineUserPrefab) as GameObject;
				text.GetComponent<Text> ().text = user.Username;
				//text.GetComponent<Text> ().color = new Color (189, 115, 22);
				//text.GetComponent<Text> ().fontSize = 25;
				//text.GetComponent<Text> ().fontStyle = FontStyle.BoldAndItalic;
				text.transform.SetParent (onlineUserPanel.transform, false);
				text.GetComponent<connectButton> ().popUpPanel = popUpPanel;
				text.GetComponent<connectButton> ().webmanager = this;
				text.GetComponent<connectButton> ().linkedUser = user;
				//text.GetComponent<connectButton>().inputButtonPanel = inputButtonPanel;
			}
		}
	}

	void updateFriendsText(List<User> friends){
		if (friendsText.activeInHierarchy) {
			Text usertext = friendsText.GetComponent<Text> ();
			string userString = "";
			foreach (User user in friends) {
				userString += user.Username + " " + user.levelProgress + " " + user.Online + "\n";
			}
			usertext.text = userString;
		}
	}

	void messageReceived(JSONNode message){
		popUpPanel.SetActive (true);
		//inputButtonPanel.SetActive (false);
		popUpPanel.GetComponent<messagePopup> ().readMessage (message);
	}
}
