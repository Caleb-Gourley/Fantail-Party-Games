using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BombSpawner : NetworkBehaviour, INetworkPrefabInstanceHandler
{
    [Header("Spawn Settings")]
    public GameObject prefabToSpawn;
    public bool SpawnAutomatically;

    [Header("Timer Settings")]
    [Tooltip("Minimun length of bomb timer")]
    public float minLength = 15;
    [Tooltip("Maximum length of bomb timer")]
    public float maxLength = 30;
    [Tooltip("Time before bomb respawns")]
    public float timeBeforeRespawn = 2;
    public Material bombMaterial;
    private float timer;
    private float timerStart;

    private GameObject prefabInstance;
    private NetworkObject spawnedNetworkObject;
    private BombExplosion bombExplosion;
    private bool gameOver = false;


    private void Start()
    {

        prefabInstance = Instantiate(prefabToSpawn);
        spawnedNetworkObject = prefabInstance.GetComponent<NetworkObject>();
        prefabInstance.SetActive(false);
        bombExplosion = FindObjectOfType<BombExplosion>();
    }

    private void FixedUpdate()
    {
        if(prefabInstance.activeSelf)
        {
            timer -= Time.deltaTime;
            bombMaterial.color = Color.green;
            if(timer < timerStart / 3 * 2 && timer > timerStart / 3)
            {
                bombMaterial.color = Color.yellow;
            }
            else if(timer < timerStart / 3 && timer > 0)
            {
                bombMaterial.color = Color.red;
            }
            else if(timer <= 0)
            {
                StartCoroutine(DespawnTimer());
            }
        }
    }

    private void StartTimer()
    {
        timerStart = Random.Range(minLength, maxLength);
        timer = timerStart;
    }

    public void Stop()
    {
        gameOver = true;
    }

    private IEnumerator DespawnTimer()
    {
        bombExplosion.OnBombExplosion(prefabInstance);
        spawnedNetworkObject.Despawn();
        StartCoroutine(SpawnTimer());
        yield break;
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(timeBeforeRespawn);
        if(gameOver)
        {
            yield break;
        }
        SpawnInstance();
        prefabInstance.GetComponent<BombHeatSeeking>().FindNewPlayer();
        yield break;
    }

    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        prefabInstance.SetActive(true);
        prefabInstance.transform.position = transform.position;
        prefabInstance.transform.rotation = transform.rotation;
        return spawnedNetworkObject;
    }

    public void Destroy(NetworkObject networkObject)
    {
        prefabInstance.SetActive(false);
    }

    public void SpawnInstance()
    {
        if(!IsServer)
        {
            return;
        }

        Vector3 spawnLocation = GetSpawnLocation();
        gameOver = false;

        if(prefabInstance != null && spawnedNetworkObject != null && !spawnedNetworkObject.IsSpawned)
        {
            prefabInstance.transform.position = spawnLocation;
            prefabInstance.SetActive(true);
            spawnedNetworkObject.Spawn();
            StartTimer();
        }
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.PrefabHandler.AddHandler(prefabToSpawn, this);

        if (!IsServer || !SpawnAutomatically)
        {
            return;
        }

        if (SpawnAutomatically)
        {
            SpawnInstance();
        }
    }

    public override void OnNetworkDespawn()
    {
        if(spawnedNetworkObject != null && spawnedNetworkObject.IsSpawned)
        {
            spawnedNetworkObject.Despawn();
        }

        base.OnNetworkDespawn();
    }

    public override void OnDestroy()
    {
        if (prefabInstance != null)
        {
            NetworkManager.Singleton.PrefabHandler.RemoveHandler(prefabToSpawn);
            Destroy(prefabInstance);
        }

        base.OnDestroy();
    }

    private Vector3 GetSpawnLocation()
    {
        float minRadius = 0.0f;
        var prefabBounds = Utilities.GetPrefabBounds(prefabToSpawn);
        if(prefabBounds.HasValue)
        {
            minRadius = Mathf.Min(-prefabBounds.Value.min.x, -prefabBounds.Value.min.z, prefabBounds.Value.max.x, prefabBounds.Value.max.z);
            if (minRadius < 0f)
            {
                minRadius = 0f;
            }
        }

        foreach(var room in MRUK.Instance.Rooms)
        {
            var randomPos = room.GenerateRandomPositionInRoom(minRadius, true);
            if(!randomPos.HasValue)
            {
                break;
            }
            
            return randomPos.Value;
        }
        
        return prefabInstance.transform.position;
    }
}
