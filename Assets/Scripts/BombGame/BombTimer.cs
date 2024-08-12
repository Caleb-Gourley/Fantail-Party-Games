using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BombTimer : MonoBehaviour
{

    [SerializeField] private PlayersInBombRangeTracker playerInBombRangeTracker;
    [SerializeField] private float minTime, maxTime;
    private float timerLength;

    GameObject playerManager;
    private BombSpawner bombSpawner;
    private NetworkManager networkManager;
    private List<GameObject> playersList;
    private List<GameObject> tempPlayersList;

    private GameObject chosenPlayerTarget;

    public float bombSpeed = 0.25f;

    public float forceMagnitude = 10f;
    private Rigidbody rb;

    private bool canSpawnObject = false;
    //private LineRenderer lineRenderer;

    //public GameObject debugPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();

        playerManager = GameObject.Find("PlayerManager");
        bombSpawner = FindObjectOfType<BombSpawner>();
        networkManager = FindObjectOfType<NetworkManager>();

        networkManager.OnServerStarted += ServerStarted;

        playersList = playerManager.GetComponent<BombPlayersManager>().GetPlayersList();
        Debug.Log("PLAYERSSSSSS IN LIST: " + playersList.Count);

        tempPlayersList = new List<GameObject>();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        timerLength = Random.Range(minTime, maxTime);
        Debug.Log(timerLength);

        chosenPlayerTarget = playerManager.GetComponent<BombPlayersManager>().RandomSelectPlayerFromList();

        ///playersList = new List<GameObject>();

        ///addPlayersToList();
        bombSpawner.bombs.Add(this.gameObject);
    }

    // Update is called once per frame

    void Update()
    {

        if(!GetComponent<NetworkObject>().IsSpawned)
        {
            GetComponent<NetworkObject>().Spawn();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootLinecast();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            KickToPlayer();
        }

        transform.position = Vector3.MoveTowards(transform.position, chosenPlayerTarget.transform.position, bombSpeed * Time.deltaTime);

        //if (playersList.Count <= 0)
        //{
        //    Debug.Log("ALL Players Eliminated...RESTARTING");
        //    addPlayersToList();
        //}

        timerLength -= Time.deltaTime;
        if(timerLength <= 0)
        {
            Debug.Log("Exploded");
            timerLength = 5f;
            Debug.Log("BOMB TIMER LENGTH: " + timerLength);
            ResetBomb();
        }
    }

    void ResetBomb()
    {
        ShootLinecast();

        timerLength = Random.Range(minTime, maxTime);
        Debug.Log("BOMB TIMER LENGTH: " + timerLength);

        Debug.Log(tempPlayersList.Count);

        playerManager.GetComponent<BombPlayersManager>().SetTempPlayersList(tempPlayersList);

        playerManager.GetComponent<BombPlayersManager>().SwapTempPlayerListToPlayerList();

        //Debug.Log("XXXXXXXXXXXXXXXXXXXXXXXXXX");

        //playersList = tempPlayersList;
        //Debug.Log("PLAYER COUNT: " + playersList.Count);


        //GetComponent<NetworkObject>().Despawn();
        bombSpawner.bombs.Remove(this.gameObject);
        GetComponent<NetworkObject>().Despawn();

        


        //if (playerInBombRangeTracker != null)
        //{
        //    foreach (GameObject player in playerInBombRangeTracker.playersInRange)
        //    {
        //        Debug.Log("Player eliminated " + player.name);
        //    }
        //}
    }

    void ServerStarted()
    {
        canSpawnObject = true;
    }

    //void addPlayersToList()
    //{
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    foreach  (GameObject player in players)
    //    {
    //        if (!checkIfPlayerInList(player))
    //        {
    //            playersList.Add(player);
    //        }
    //    }


    //    Debug.Log(playersList.Count);

    //    ///Debugging Purposes:
    //    //foreach(GameObject player in playersList)
    //    //{
    //    //    Debug.Log(player.name);
    //    //}
    //}

    //bool checkIfPlayerInList(GameObject player)
    //{
    //    if (playersList.Contains(player))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    void ShootLinecast()
    {
        foreach (GameObject player in playersList)
        {
            RaycastHit hit; Physics.Linecast(transform.position, player.transform.position, out hit);

            Debug.DrawLine(transform.position, player.transform.position, Color.red, 4.0f);

            if (hit.collider.tag == "Wall")
            {
                Debug.Log("LINECAST HIT  WALL: " + hit.collider.name);

                Debug.Log("ADD TO TEMP LIST");
                tempPlayersList.Add(player);
            }
            //else
            //{
                
            //}
        }

    }

    void KickToPlayer()
    {

        //Vector3 normalizedDirection = (debugPlayer.transform.position-transform.position).normalized;

        //rb.AddForce(normalizedDirection * forceMagnitude, ForceMode.Impulse);

    }
}