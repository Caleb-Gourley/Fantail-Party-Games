using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BombPlayersManager : MonoBehaviour
{
    private List<GameObject> playersList;
    private List<GameObject> tempPlayersList;
    private NetworkManager networkManager;
    public bool serverStarted = false;


    // Start is called before the first frame update
    void Start()
    {
        playersList = new List<GameObject>();
        networkManager = FindAnyObjectByType<NetworkManager>();

        networkManager.OnServerStarted += OnServerStarted;
        addPlayersToList();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetPlayersList()
    {
        return playersList;
    }

    public void SetTempPlayersList(List<GameObject> list)
    {
        tempPlayersList = list;
    }

    public void SwapTempPlayerListToPlayerList()
    {
        playersList.Clear();
        if (tempPlayersList.Count != 0)
        {
            playersList.AddRange(tempPlayersList);
            tempPlayersList.Clear();
        }
        

        if (playersList.Count <= 0)
        {
            // Debug.Log("ALL Players Eliminated...RESTARTING");
            addPlayersToList();
        }
    }

    void addPlayersToList()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (!checkIfPlayerInList(player))
            {
                playersList.Add(player);
            }
        }


        // Debug.Log("PLAYERZZZZZZ LIST: " + playersList.Count);

        ///Debugging Purposes:
        //foreach(GameObject player in playersList)
        //{
        //    Debug.Log(player.name);
        //}
    }

    public GameObject RandomSelectPlayerFromList()
    {
        int randomIndex = Random.Range(0, playersList.Count);
        // Debug.Log("plyaer chosen index: " + randomIndex);
        return playersList[randomIndex];
    }

    bool checkIfPlayerInList(GameObject player)
    {
        if (playersList.Contains(player))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnServerStarted()
    {
        serverStarted = true;
    }
}
