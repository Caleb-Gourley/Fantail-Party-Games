using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BombTimer : MonoBehaviour
{

    [SerializeField] private PlayersInBombRangeTracker playerInBombRangeTracker;
    [SerializeField] private float minTime, maxTime;
    private float timerLength;
    private List<GameObject> playersList;
    //private LineRenderer lineRenderer;

    public GameObject debugPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();

        timerLength = Random.Range(minTime, maxTime);
        Debug.Log(timerLength);

        playersList = new List<GameObject>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playersList.AddRange(players);

        Debug.Log(playersList.Count);
        
        ///Debugging Purposes:
        //foreach(GameObject player in playersList)
        //{
        //    Debug.Log(player.name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootLinecast();
        }




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

        //GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);

        


        //if (playerInBombRangeTracker != null)
        //{
        //    foreach (GameObject player in playerInBombRangeTracker.playersInRange)
        //    {
        //        Debug.Log("Player eliminated " + player.name);
        //    }
        //}
    }

    void ShootLinecast()
    {
        foreach (GameObject player in playersList)
        {
            RaycastHit hit; Physics.Linecast(transform.position, player.transform.position, out hit);

            Debug.DrawLine(transform.position, player.transform.position, Color.red, 4.0f);

            if (hit.collider.tag == "Wall")
            {
                Debug.Log("LINECAST HIT  WALL: " + hit.collider.name);
            }
        }

    }
}