using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (Application.loadedLevel != 1) {
			base.OnServerAddPlayer(conn, playerControllerId);
		}
	}
}
