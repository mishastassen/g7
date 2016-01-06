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
        var spawn = Random.Range(0, 4);
        var spawn2 = Random.Range(4, 8);

        if (Timer < 0)
        {
            Timer = SpawnTime;
            SpawnObject(spawn,spawn2);
        }
    }
    void SpawnObject(int a, int x)
    {
        ClonetoSpawn[a] = Instantiate(PrefabtoSpawn[a], Spawnlocations[a].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        ClonetoSpawn[x] = Instantiate(PrefabtoSpawn[x], Spawnlocations[x].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }
}

