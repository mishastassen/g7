using System;
using UnityEngine;
using SimpleJSON;

public class User{

	public int UserId;
	public string Username;
	public string ip;
	public int levelProgress;
	public bool Online;

	public User (int UserId, string Username, int levelProgress,bool Online){
		this.UserId = UserId;
		this.Username = Username;
		this.levelProgress = levelProgress;
		this.Online = Online;
	}

	public User(JSONNode aJSON){
		UserId = aJSON ["UserId"].AsInt;
		Username = aJSON ["Username"].Value;
		levelProgress = aJSON ["levelProgress"].AsInt;
		Online = aJSON ["LoggedIn"].AsBool;
	}

	public void setIp(string ip){
		this.ip = ip;
	}
}
