using UnityEngine;
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
					return static_instance;
				}
				if(static_instance == null){
					GameObject singleton = new GameObject();
					singleton.AddComponent<NetworkIdentity>();
					static_instance = singleton.AddComponent<Gamemanager>();
					singleton.name = "(singleton) "+ typeof(Gamemanager).ToString();
					DontDestroyOnLoad(singleton);
					NetworkServer.Spawn (singleton);
				}
			}
			return static_instance;
		}
	}

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Start () {
		
	}

	void Update () {
		if (localmultiplayer && ClientScene.localPlayers[2].gameObject == null && ClientScene.ready) {
			ClientScene.AddPlayer(2);
		}
	}

	public void OnDestroy () {
		applicationIsQuitting = true;
	}
	
}
