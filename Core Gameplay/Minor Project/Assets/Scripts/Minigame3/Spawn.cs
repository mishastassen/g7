using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Spawn : NetworkBehaviour
{
    private int b;
    private float Timer;
    private float obstaclesspawnded;
    private bool stop;

	public Image winPlank;
    public Text winText;
	public Text winInstruction;
    public float obstaclestododge;
    public float SpawnTime; //kan later private
    public Transform[] Spawnlocations;
    public GameObject[] PrefabtoSpawn;

    void Start()
    {
        Timer = 0;
        b = 3;
        obstaclesspawnded = 0;
        stop = false;
		winPlank.enabled = false;
		winText.enabled = false;
		winInstruction.enabled = false;
    }

    void Update()
    {

        //timer
        Timer -= Time.deltaTime;
        //rng
        var spawn = Random.Range(0, 4);
        var spawn2 = Random.Range(4, 8);

        if (Timer < 0 && !stop)
        {
			Timer = (1.5f- obstaclesspawnded / obstaclestododge) * SpawnTime;
            SpawnObject(spawn, spawn2);
            obstaclesspawnded = obstaclesspawnded + 1;
            stopspawn();
        }

        //display win
        if (Timer < -5 && stop)
        {
			winPlank.enabled = true;
			winText.enabled = true;
			winInstruction.enabled = true;
        }
    }

    void SpawnObject(int a, int x)
    {
        if (isServer)
        {
            GameObject clonedSpawn1 = Instantiate(PrefabtoSpawn[a], Spawnlocations[a].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            GameObject clonedSpawn2 = Instantiate(PrefabtoSpawn[x], Spawnlocations[x].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            NetworkServer.Spawn(clonedSpawn1);
            NetworkServer.Spawn(clonedSpawn2);
        }
    }


    void stopspawn()
    {
        if (obstaclesspawnded.Equals(obstaclestododge))
        {
            stop = true;
        }
    }


}