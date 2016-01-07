using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Destructionwall : NetworkBehaviour
{
    private Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
		if (isServer) {
			Destroy (other.gameObject);
		}
    }
}
