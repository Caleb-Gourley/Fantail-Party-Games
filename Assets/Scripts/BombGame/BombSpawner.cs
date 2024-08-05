using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bomb;
    public GameObject bombSpawnpoint;
    // Start is called before the first frame update
    void Start()
    {
        SpawnBomb();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBomb()
    {
        var currentBomb = Instantiate(bomb, bombSpawnpoint.transform).GetComponent<NetworkObject>();
        currentBomb.Spawn(); 
    }
}
