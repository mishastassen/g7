using UnityEngine;
using System.Collections;

public class Destructionwall : MonoBehaviour
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
        Destroy(other.gameObject);
    }
}
