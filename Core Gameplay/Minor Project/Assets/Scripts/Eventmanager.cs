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
	/*
	//Magic Package pickup event
	public delegate void PackagePickupMagic(NetworkInstanceId netId,string tag);
	[SyncEvent]
	public event PackagePickupMagic EventonPackagePickupMagic;
	*/
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

	//Level finished
	public delegate void LevelFinished(string nextLevel);
	public event LevelFinished EventonLevelFinished;

	//Chest activated
	public delegate void ChestActivated();
	public event ChestActivated EventonChestActivated;

	//Chest completed succesfully
	public delegate void ChestCompleted();
	public event ChestActivated EventonChestCompleted;

	//Function to call this object
	public static Eventmanager Instance{
		get{
			if (applicationIsQuitting) {
				return null;
			}
			lock(_lock)
			{
				static_instance = (Eventmanager) FindObjectOfType(typeof(Eventmanager));

				if ( FindObjectsOfType(typeof(Eventmanager)).Length > 1 )
				{
					return static_instance;
				}

				if(static_instance == null){
					GameObject singleton = new GameObject();
					singleton.AddComponent<NetworkIdentity>();
					static_instance = singleton.AddComponent<Eventmanager>();
					singleton.name = "(singleton) "+ typeof(Eventmanager).ToString();
					DontDestroyOnLoad(singleton);
					NetworkServer.Spawn (singleton);
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
		if (!Gamemanager.Instance.packageheld) {
			Gamemanager.Instance.packageheld = true;
			Gamemanager.Instance.packageholder = player.GetComponent<NetworkIdentity> ().netId;
			EventonPackagePickup (Gamemanager.Instance.packageholder,tag);
		}
	}
	/*
	//Trigger when player tries to pick up magic package
	public void packagePickupMagic(GameObject player,string tag){
		if (!Gamemanager.Instance.packageheld) {
			Gamemanager.Instance.packageheld = true;
			Gamemanager.Instance.packageholder = player.GetComponent<NetworkIdentity> ().netId;
			EventonPackagePickupMagic (Gamemanager.Instance.packageholder,tag);
		}
	}
	*/
	//Trigger when player tries to drop package
	public void packageDrop(GameObject player){
		if (Gamemanager.Instance.packageholder == player.GetComponent<NetworkIdentity> ().netId) {
			Gamemanager.Instance.packageheld = false;
			EventonPackageDrop (Gamemanager.Instance.packageholder);
		}
	}

	//Trigger when player tries to thorw package
	public void packageThrow(GameObject player){
		if (Gamemanager.Instance.packageholder == player.GetComponent<NetworkIdentity> ().netId) {
			Gamemanager.Instance.packageheld = false;
			EventonPackageThrow (Gamemanager.Instance.packageholder);
		}
	}

	//Trigger when player dies
	public void triggerPlayerDeath(GameObject player){
		if (Gamemanager.Instance.packageholder == player.GetComponent<NetworkIdentity> ().netId && Gamemanager.Instance.packageheld == true) {
			triggerPackageDestroyed ();
		}
		EventonPlayerDeath (player);
	}

	//Trigger when player is destroyed
	public void triggerPackageDestroyed(){
		Gamemanager.Instance.packageheld = false;
		if (EventonPackageDestroyed != null) { //Don't execute if noone is listening to event
			EventonPackageDestroyed ();
		}
	}

	//Trigger when level is finished(){
	public void triggerLevelFinished(string nextLevel){
		if (EventonLevelFinished != null) { //Don't execute if noone is listening to event
			EventonLevelFinished(nextLevel);
		}
	}

	public void triggerChestActivated(){
		if (EventonChestActivated != null) { //Don't execute if noone is listening to event
			EventonChestActivated();
		}
	}

	public void triggerChestCompleted(){
		if (EventonChestCompleted != null) { //Don't execute if noone is listening to event
			EventonChestCompleted();
		}
	}
}
