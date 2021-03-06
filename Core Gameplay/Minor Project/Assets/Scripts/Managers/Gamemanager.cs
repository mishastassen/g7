using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Gamemanager : NetworkBehaviour {

	private static Gamemanager static_instance = null;
	private static object _lock = new object ();

	//Game vars
	[SyncVar]
	public bool packageheld;
	[SyncVar]
	public NetworkInstanceId packageholder;
	[SyncVar]
	public bool localmultiplayer;
	[SyncVar]
	public int CheckpointReached;
	//public string currentLevel;
	//public float timer;

	public bool disabledManager = false;

	//Executed on the server when next level is loaded
	public delegate void OnNextLevelLoad();
	public OnNextLevelLoad onNextLevelLoad;

	//Disable eventhandlers on level finish
	public delegate void disableEventHandlers();
	public disableEventHandlers onDisableEventHandlers;

	//References
	private NetworkManager networkmanager;

	//timer
	private float playercheckTimer;

	//Function to call this object
	public static Gamemanager Instance{
		get{
			lock(_lock)
			{
				static_instance = (Gamemanager) FindObjectOfType(typeof(Gamemanager));
				
				if ( FindObjectsOfType(typeof(Gamemanager)).Length > 1 )
				{
					Debug.LogError("Warning: multiple game managers");
					return static_instance;
				}
				if(static_instance == null){
					//Debug.LogError("Error no Gamemanager singleton");
				}
			}
			return static_instance;
		}
	}

	void Awake () {
		DontDestroyOnLoad (gameObject);
		networkmanager = (NetworkManager) FindObjectOfType(typeof(NetworkManager));
	}
		
	void Update () {
		if (NetworkManager.networkSceneName == "preScene") {
			if (WebManager.Instance != null) {
				if (WebManager.Instance.localmultiplayer) {
					localmultiplayer = true;
					ClientScene.AddPlayer (2);
					Eventmanager.Instance.triggerLevelSwitch (WebManager.Instance.level1);
				} else {
					Eventmanager.Instance.triggerLevelSwitch (WebManager.Instance.level1);
				}
			}
		} else if (localmultiplayer && ClientScene.localPlayers [2].gameObject == null && ClientScene.ready) {
			ClientScene.AddPlayer (2);
		} else if (!localmultiplayer && ClientScene.ready && playercheckTimer > 3 && !isServer) {
			if (NetworkManager.singleton.client.connection.playerControllers [0] == null) {
			//	ClientScene.AddPlayer (NetworkManager.singleton.client.connection, 0);
			}
			playercheckTimer = 0;
		}
		playercheckTimer += Time.deltaTime;
	}

	[ServerCallback]
	void OnLevelWasLoaded(int level){
		if (onNextLevelLoad != null) {
			onNextLevelLoad ();
		}
		onNextLevelLoad = null;
	}

	public void triggerDisableEventHandlers(){
		if (onDisableEventHandlers != null) {
			onDisableEventHandlers ();
		}
		onDisableEventHandlers = null;
	}

	public void stopManager(){
		networkmanager = null;
		static_instance = null;
		disabledManager = true;
		gameObject.SetActive (false);
	}
}
