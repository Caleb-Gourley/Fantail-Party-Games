using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bomb;
    public GameObject bombSpawnpoint;
    public float timeToWaitBeforeSpawn;
    private GameObject activeBomb;

    private bool spawningBomb = false;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnBomb();
    }

    // Update is called once per frame
    void Update()
    {
        if(activeBomb == null && !spawningBomb)
        {
            StartCoroutine(StartBombSpawn());
        }
    }

    void SpawnBomb()
    {
        activeBomb = Instantiate(bomb, bombSpawnpoint.transform);
        activeBomb.GetComponent<NetworkObject>().Spawn(); 
    }

    IEnumerator StartBombSpawn()
    {
        spawningBomb = true;
        yield return new WaitForSeconds(timeToWaitBeforeSpawn);
        SpawnBomb();
        spawningBomb = false;
    }
}
