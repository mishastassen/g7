using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BaseGuard : NetworkBehaviour {

	public static float getStrength() {
		float strength= 10 - Gamevariables.playersDeathCount + Gamevariables.guardsDeathCount;
		strength = Mathf.Clamp (strength, 3, 13);
		strength /= 10;
		return strength;
	}

}
