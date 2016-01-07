using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class minigame2Controller : NetworkBehaviour {

	// walkspeed, windspeed, rotatespeed, blowdir, severity
	public float windspeed;
	[SyncVar][HideInInspector]
	public float blowdir;
	[SyncVar][HideInInspector]
	public float severity;
	[SyncVar][HideInInspector]
	public float compensate;
	int frame_count;
	[SyncVar][HideInInspector]
	int wind_count;

	// is the wind blowing?
	[SyncVar][HideInInspector]
	public bool windBlowing;
	public ParticleSystem toleft;
	public ParticleSystem toright;

	[HideInInspector]
	public GameObject left;
	[HideInInspector]
	public GameObject right;

	void Start () {
		severity = 1.0f;
		compensate = 1.5f;
		frame_count = 1;
		toleft.enableEmission = false;
		toright.enableEmission = false;

		if (isServer) {
			blowdir = getWindDirection ();
		}
	}

	

	void Update () {
		if (isServer) {
			frame_count++;
			if (frame_count % 100 == 0) {
				windBlowing = true;
			}

			if (windBlowing && wind_count > 40) {
				windBlowing = false;
				wind_count = 0;
				blowdir = getWindDirection();
				// geluid.Stop ();
			}

			// increase the severity of the wind if one progresses
			if (left != null && right != null) {
				if (left.transform.position.z > 15 | right.transform.position.z > 15) {
					severity = 1.25f;
					compensate = severity * 1.4f;
				}

				if (left.transform.position.z > 40 | right.transform.position.z > 40) {
					severity = 1.5f;
					compensate = severity * 1.3f;
				}

				if (left.transform.position.z > 60 | right.transform.position.z > 60) {
					severity = 2.0f;
					compensate = severity * 1.1f;
				}
			}
		}

		if (windBlowing) {
			// geluid.PlayOneShot(wind,1.0f);
			wind_count++;
			if (blowdir > 0) {
				toleft.enableEmission = true;
				toleft.Play ();
			} else {
				toright.enableEmission = true;
				toright.Play ();
			}
		} else {
			toleft.enableEmission = false;
			toright.enableEmission = false;
		}
	}

	float getWindDirection(){
		blowdir = Random.Range (-1.0f, 1.0f);

		if (blowdir < 0) {
			blowdir = -1;
		} else if (blowdir > 0) {
			blowdir = 1;
		} else {
			blowdir = getWindDirection();
		}
		return blowdir;
	}
}
