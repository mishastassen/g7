using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BaseGuard : NetworkBehaviour {

	public static float getStrength() {
		float strength= Mathf.Max(5,10 - Gamevariables.deathCount);
		strength /= 10;
		return strength;
	}

}
