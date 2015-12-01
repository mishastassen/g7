using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Eventmanager : NetworkBehaviour {
	
	private static Eventmanager static_instance = null;

	//Events:
	//Playeradded event
	public delegate void PlayerAdded(GameObject player);
	public static event PlayerAdded onPlayerAdded;

	//PlayerRemoved event
	public delegate void PlayerRemoved(GameObject player);
	public static event PlayerRemoved onPlayerRemoved;

	//SwitchPulled event
	public delegate void SwitchPulled();
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
			if(static_instance == null){
				static_instance = GameObject.FindObjectOfType(typeof(Eventmanager)) as Eventmanager;
			}
			return static_instance;
		}
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
	public void triggerSwitchPulled(){
		if (EventonSwitchPulled != null) {	//Don't execute if noone is listening to event
			EventonSwitchPulled();
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
