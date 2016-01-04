using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    private int b;
    private float Timer;

    public float SpawnTime; //kan later private
    public Transform[] Spawnlocations;
    public GameObject[] PrefabtoSpawn;
    public GameObject[] ClonetoSpawn;

    void Start()
    {
        Timer = 0;
        b = 3;
    }

    void FixedUpdate()
    {

        //timer
        Timer -= Time.deltaTime;
        //rng
        var randomInt = Random.Range(0, 3);
        b = randomInt;

        if (Timer < 0)
        {
            Timer = SpawnTime;
            SpawnObject(b);
        }
    }
    void SpawnObject(int a)
    {
        ClonetoSpawn[a] = Instantiate(PrefabtoSpawn[a], Spawnlocations[a].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }
}

