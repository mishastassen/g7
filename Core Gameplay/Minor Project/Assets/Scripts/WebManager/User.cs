using System;
using UnityEngine;
using SimpleJSON;

public class User{

	public int UserId;
	public string Username;
	public string Ip;
	public int levelProgress;
	public bool Online;
	public Color playerColor;
	public string Sex;

	public User (int UserId, string Username, int levelProgress,bool Online,string Ip, Color playerColor,string Sex){
		this.UserId = UserId;
		this.Username = Username;
		this.levelProgress = levelProgress;
		this.Online = Online;
		this.Ip = Ip;
		this.playerColor = playerColor;
		this.Sex = Sex;
	}

	public User(JSONNode aJSON){
		UserId = aJSON ["UserId"].AsInt;
		Username = aJSON ["Username"].Value;
		levelProgress = aJSON ["levelProgress"].AsInt;
		Online = aJSON ["LoggedIn"].AsBool;
		Ip = aJSON ["ipInfo"]["clientIp"];
		string hex = aJSON ["playerColor"];
		playerColor = hexToColor (hex);
		Sex = aJSON ["Sex"];
	}

	private Color hexToColor(string hex)
	{
		hex = hex.Replace ("#", "");
		byte a = 255;
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b,a);
	}
}
