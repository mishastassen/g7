using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Eventmanager : NetworkBehaviour {
	
	private static Eventmanager static_instance = null;
	private static object _lock = new object ();
	private static bool applicationIsQuitting = false;

	//Events:
	//Playeradded event
	public delegate void PlayerAdded(GameObject player);
	public static event PlayerAdded onPlayerAdded;

	//PlayerRemoved event
	public delegate void PlayerRemoved(GameObject player);
	public static event PlayerRemoved onPlayerRemoved;

	//SwitchPulled event
	public delegate void SwitchPulled(int id);
	//[SyncEvent]
	public event SwitchPulled EventonSwitchPulled;

	//Package pickup event
	public delegate void PackagePickup(NetworkInstanceId netId,string tag);
	//[SyncEvent]
	public event PackagePickup EventonPackagePickup;

	//Package drop event
	public delegate void PackageDrop(NetworkInstanceId netId);
	//[SyncEvent]
	public event PackageDrop EventonPackageDrop;

	//Package throw event
	public delegate void PackageThrow(NetworkInstanceId netId);
	//[SyncEvent]
	public event PackageThrow EventonPackageThrow;

	//Player death event
	public delegate void PlayerDeath(GameObject player);
	public event PlayerDeath EventonPlayerDeath;

	//Player spotted event
	public delegate void PlayerSpotted();
	//[SyncEvent]
	public event PlayerSpotted EventonPlayerSpotted;

	//Player not spotted event
	public delegate void NoPlayerSpotted();
	//[SyncEvent]
	public event NoPlayerSpotted EventonNoPlayerSpotted;

	//Update alarm event
	public delegate void UpdateAlarm(string alarmString);
	public event UpdateAlarm EventonUpdateAlarm;

	//Package destroyed event
	public delegate void PackageDestroyed();
	//[SyncEvent]
	public event PackageDestroyed EventonPackageDestroyed;

	//Level finished
	public delegate void LevelFinished(string nextLevel);
	//[SyncEvent]
	public event LevelFinished EventonLevelFinished;

	//Minigame started
	public delegate void MinigameStarted();
	public event MinigameStarted EventonMinigameStarted;

	//Minigame finished
	public delegate void MinigameFinished();
	public event MinigameFinished EventonMinigameFinished;

	//Chest activated
	public delegate void ChestActivated();
	public event ChestActivated EventonChestActivated;

	//Chest completed succesfully
	public delegate void ChestCompleted();
	public event ChestActivated EventonChestCompleted;

	//CheckPoint reached
	public delegate void CheckpointReached(int checkpointNum);
	//[SyncEvent]
	public event CheckpointReached EventonCheckpointReached;

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
					Debug.LogError("Warning: multiple event managers");
					return static_instance;
				}

				if(static_instance == null){
					Debug.LogError("Error no Event manager singleton");
				}
			}
			return static_instance;
		}
	}

	void Awake(){
		DontDestroyOnLoad(gameObject);
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
			if (isServer) {
				RpcOnSwitchPulled (id);
			}
		}
	}

	//Trigger when player tries to pick up package
	public void packagePickup(GameObject player,string tag){
		if (!Gamemanager.Instance.packageheld) {
			Gamemanager.Instance.packageheld = true;
			Gamemanager.Instance.packageholder = player.GetComponent<NetworkIdentity> ().netId;
			if(EventonPackagePickup != null){
				if (isServer) {
					RpcPackagePickup (Gamemanager.Instance.packageholder,tag);
				}
			}
		}
	}

	//Trigger when player tries to drop package
	public void packageDrop(GameObject player){
		if (Gamemanager.Instance.packageholder == player.GetComponent<NetworkIdentity> ().netId) {
			Gamemanager.Instance.packageheld = false;
			if (isServer) {
				RpcPackageDrop (Gamemanager.Instance.packageholder);
			}
		}
	}

	//Trigger when player tries to thorw package
	public void packageThrow(GameObject player){
		if (Gamemanager.Instance.packageholder == player.GetComponent<NetworkIdentity> ().netId) {
			Gamemanager.Instance.packageheld = false;
			if (isServer) {
				RpcPackageThrow (Gamemanager.Instance.packageholder);
			}
		}
	}

	//Trigger when player dies
	public void triggerPlayerDeath(GameObject player){
		if (Gamemanager.Instance.packageholder == player.GetComponent<NetworkIdentity> ().netId && Gamemanager.Instance.packageheld == true) {
			triggerPackageDestroyed ();
		}
		EventonPlayerDeath (player);
	}

	//Trigger when player is spotted
	public void triggerPlayerSpotted(){
		if (EventonPlayerSpotted != null) { //Don't execute if noone is listening to event
			if (isServer) {
				RpcOnPlayerSpotted ();
			}
		}
	}

	//Trigger when no player is spotted
	public void triggerNoPlayerSpotted(){
		if (EventonNoPlayerSpotted != null) { //Don't execute if noone is listening to event
			if (isServer) {
				RpcOnNoPlayerSpotted ();
			}
		}
	}

	//Trigger when alarm has to be updated
	public void triggerUpdateAlarm(string alarmString) {
		if (EventonUpdateAlarm != null) { //Don't execute if noone is listening to event
			EventonUpdateAlarm (alarmString);
		}
	}

	//Trigger when player is destroyed
	public void triggerPackageDestroyed(){
		Gamemanager.Instance.packageheld = false;
		if (EventonPackageDestroyed != null) { //Don't execute if noone is listening to event
			//EventonPackageDestroyed ();
			if (isServer) {
				RpcOnPackageDestroyed ();
			}
		}
	}

	//Trigger when level is finished(){
	public void triggerLevelFinished(string nextLevel){
		if (EventonLevelFinished != null) { //Don't execute if noone is listening to event
			EventonLevelFinished(nextLevel);
			if (isServer) {
				RpcOnLevelFinished (nextLevel);
			}
		}
	}

	//Trigger when minigame is started{
	public void triggerMinigameStarted(){
		if (EventonMinigameStarted != null) { //Don't execute if noone is listening to event
			EventonMinigameStarted();
		}
	}

	//Trigger when minigame is finished{
	public void triggerMinigameFinished(){
		if (EventonMinigameFinished != null) { //Don't execute if noone is listening to event
			EventonMinigameFinished();
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

	public void triggerCheckpointReached(int checkpointNum){
		if (EventonCheckpointReached != null) {
			if (isServer) {
				RpcOnCheckpointReached (checkpointNum);
			}
		}
	}

	[ClientRpc]
	void RpcOnSwitchPulled(int id){
		if (EventonSwitchPulled != null) {
			EventonSwitchPulled (id);
		}
	}

	[ClientRpc]
	void RpcPackagePickup(NetworkInstanceId player, string tag){
		if (EventonPackagePickup != null) {
			EventonPackagePickup (player, tag);
		}
	}

	[ClientRpc]
	void RpcPackageDrop(NetworkInstanceId player){
		if (EventonPackageDrop != null) {
			EventonPackageDrop (player);
		}
	}

	[ClientRpc]
	void RpcPackageThrow(NetworkInstanceId player){
		if (EventonPackageThrow != null) {
			EventonPackageThrow (player);
		}
	}

	[ClientRpc]
	void RpcOnPlayerSpotted (){
		if (EventonPlayerSpotted != null) {
			EventonPlayerSpotted ();
		}
	}

	[ClientRpc]
	void RpcOnNoPlayerSpotted (){
		if (EventonNoPlayerSpotted != null) {
			EventonNoPlayerSpotted ();
		}
	}

	[ClientRpc]
	void RpcOnPackageDestroyed (){
		if (EventonPackageDestroyed != null) {
			EventonPackageDestroyed ();
		}
	}

	[ClientRpc]
	void RpcOnLevelFinished (string nextLevel){
		if (EventonLevelFinished != null) {
			EventonLevelFinished (nextLevel);
		}
	}

	[ClientRpc]
	void RpcOnCheckpointReached (int checkpointNum){
		if (EventonCheckpointReached  != null) {
			EventonCheckpointReached  (checkpointNum);
		}
	}
}
