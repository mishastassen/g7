using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Gamemanager : NetworkBehaviour {

	private static Gamemanager static_instance = null;
	private static int singleton_count;

	public bool localmultiplayer; 
	private NetworkManager networkmanager;

	//Function to call this object
	public static Gamemanager Instance{
		get{
			if(static_instance == null){
				static_instance = GameObject.FindObjectOfType(typeof(Gamemanager)) as Gamemanager;
			}
			return static_instance;
		}
	}

	void Awake() {
		singleton_count++;
		if (singleton_count > 1)
		{
			DestroyImmediate(this.gameObject);
			return;
		}
		DontDestroyOnLoad(transform.gameObject);
		networkmanager = GameObject.Find("Network manager").GetComponent<NetworkManager>();
	}
	
	void Start () {
		
	}

	void Update () {
		if (localmultiplayer && ClientScene.localPlayers[2].gameObject == null && ClientScene.ready) {
			ClientScene.AddPlayer(2);
		}
	}

	void OnDestroy(){
		singleton_count--;
	}
}
