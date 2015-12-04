﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Gamemanager : NetworkBehaviour {

	private static Gamemanager static_instance = null;
	private static object _lock = new object ();
	private static bool applicationIsQuitting = false;

	//Game vars
	[SyncVar]
	public bool packageheld;
	[SyncVar]
	public NetworkInstanceId packageholder;
	[SyncVar]
	public bool localmultiplayer;
	
	//Executed on the server when next level is loaded
	public delegate void OnNextLevelLoad();
	public OnNextLevelLoad onNextLevelLoad;

	//Function to call this object
	public static Gamemanager Instance{
		get{
			if (applicationIsQuitting) {
				return null;
			}
			lock(_lock)
			{
				static_instance = (Gamemanager) FindObjectOfType(typeof(Gamemanager));
				
				if ( FindObjectsOfType(typeof(Gamemanager)).Length > 1 )
				{
					Debug.Log("Warning: multiple game managers");
					return static_instance;
				}
				if(static_instance == null){
					Debug.Log("Error no Gamemanager singleton");
				}
			}
			return static_instance;
		}
	}

	void Awake () {
		DontDestroyOnLoad (gameObject);
	}

	void Update () {
		if (localmultiplayer && ClientScene.localPlayers[2].gameObject == null && ClientScene.ready) {
			ClientScene.AddPlayer(2);
		}
	}

	void OnDestroy () {
		applicationIsQuitting = true;
	}

	[ServerCallback]
	void OnLevelWasLoaded(int level){
		if (onNextLevelLoad != null) {
			onNextLevelLoad ();
		}
		onNextLevelLoad = null;
	}
}