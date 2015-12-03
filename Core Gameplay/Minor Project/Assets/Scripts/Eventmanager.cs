using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Eventmanager : NetworkBehaviour {
	
	private static Eventmanager static_instance = null;
	private static object _lock = new object ();

	//Events:
	//Playeradded event
	public delegate void PlayerAdded(GameObject player);
	public static event PlayerAdded onPlayerAdded;

	//PlayerRemoved event
	public delegate void PlayerRemoved(GameObject player);
	public static event PlayerRemoved onPlayerRemoved;

	//SwitchPulled event
	public delegate void SwitchPulled(int id);
	[SyncEvent]
	public event SwitchPulled EventonSwitchPulled;

	//Package pickup event
	public delegate void PackagePickup(NetworkInstanceId netId,string tag);
	[SyncEvent]
	public event PackagePickup EventonPackagePickup;

	//Package drop event
	public delegate void PackageDrop(NetworkInstanceId netId);
	[SyncEvent]
	public event PackageDrop EventonPackageDrop;

	//Package throw event
	public delegate void PackageThrow(NetworkInstanceId netId);
	[SyncEvent]
	public event PackageThrow EventonPackageThrow;

	//Player death event
	public delegate void PlayerDeath(GameObject player);
	public event PlayerDeath EventonPlayerDeath;

	//Package destroyed event
	public delegate void PackageDestroyed();
	[SyncEvent]
	public event PackageDestroyed EventonPackageDestroyed;

	//GameVars:
	[SyncVar]
	public bool packageheld;
	[SyncVar]
	public NetworkInstanceId packageholder;

	//Function to call this object
	public static Eventmanager Instance{
		get{
			if (applicationIsQuitting) {
				Debug.LogWarning("[Singleton] Instance '"+ typeof(Eventmanager) +
				                 "' already destroyed on application quit." +
				                 " Won't create again - returning null.");
				return null;
			}
			lock(_lock)
			{
				static_instance = (Eventmanager) FindObjectOfType(typeof(Eventmanager));

				if ( FindObjectsOfType(typeof(Eventmanager)).Length > 1 )
				{
					Debug.LogError("[Singleton] Something went really wrong " +
					               " - there should never be more than 1 singleton!" +
					               " Reopening the scene might fix it.");
					return static_instance;
				}

				if(static_instance == null){
					GameObject singleton = new GameObject();
					static_instance = singleton.AddComponent<Eventmanager>();
					singleton.name = "(singleton) "+ typeof(Eventmanager).ToString();
					
					DontDestroyOnLoad(singleton);
					
					Debug.Log("[Singleton] An instance of " + typeof(Eventmanager) + 
					          " is needed in the scene, so '" + singleton +
					          "' was created with DontDestroyOnLoad.");
				}
				else {
					Debug.Log("[Singleton] Using instance already created: " +
					          static_instance.gameObject.name);
				}
			}
			return static_instance;
		}
	}

	private static bool applicationIsQuitting = false;

	public void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	public void OnDestroy () {
		applicationIsQuitting = true;
	}

	//Trigger PlayerAdded event
	public void triggerPlayerAdded(GameObject player){
		if (onPlayerAdded != null) {	//Don't execute if noone is listening to event
			onPlayerAdded(player);
		}
	}

	//Trigger PlayerRemoved event
	public void triggerPlayerRemoved(GameObject player){
		if (onPlayerRemoved != null) {	//Don't execute if noone is listening to event
			onPlayerRemoved(player);
		}
	}
	
	//Trigger SwitchPulled event
	public void triggerSwitchPulled(int id){
		if (EventonSwitchPulled != null) {	//Don't execute if noone is listening to event
			EventonSwitchPulled(id);
		}
	}

	//Trigger when player tries to pick up package
	public void packagePickup(GameObject player,string tag){
		if (!packageheld) {
			packageheld = true;
			packageholder = player.GetComponent<NetworkIdentity> ().netId;
			EventonPackagePickup (packageholder,tag);
		}
	}

	//Trigger when player tries to drop package
	public void packageDrop(GameObject player){
		if (packageholder == player.GetComponent<NetworkIdentity> ().netId) {
			packageheld = false;
			EventonPackageDrop (packageholder);
		}
	}

	//Trigger when player tries to thorw package
	public void packageThrow(GameObject player){
		if (packageholder == player.GetComponent<NetworkIdentity> ().netId) {
			packageheld = false;
			EventonPackageThrow (packageholder);
		}
	}

	//Trigger when player dies
	public void triggerPlayerDeath(GameObject player){
		if (packageholder == player.GetComponent<NetworkIdentity> ().netId && packageheld == true) {
			triggerPackageDestroyed ();
		}
		EventonPlayerDeath (player);
	}

	//Trigger when player is destroyed
	public void triggerPackageDestroyed(){
		packageheld = false;
		EventonPackageDestroyed ();
	}
}
