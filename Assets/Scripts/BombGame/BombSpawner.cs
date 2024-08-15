using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BombSpawner : NetworkBehaviour
{
    public float timeToWaitBeforeSpawn;
    public FindSpawnPositions findSpawnPositions;
    private NetworkManager networkManager;

    public List<GameObject> bombs;
    private bool spawningBomb = true;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = FindAnyObjectByType<NetworkManager>();
        networkManager.OnServerStarted += SpawnedFirstBomb;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!networkManager.IsServer)
            return;
        
        if(!spawningBomb && bombs.Count == 0)
        {
            Debug.LogError("Spawning Bomb");
            StartCoroutine(StartBombSpawn());
        }
    }

    void SpawnBomb()
    {
        findSpawnPositions.StartSpawn();
    }

    IEnumerator StartBombSpawn()
    {
        spawningBomb = true;
        yield return new WaitForSeconds(timeToWaitBeforeSpawn);
        SpawnBomb();
        spawningBomb = false;
    }

    private void SpawnedFirstBomb()
    {
        spawningBomb = false;
    }

}
