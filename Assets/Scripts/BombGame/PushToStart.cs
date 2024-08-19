using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.Netcode;
using UnityEngine;

public class PushToStart : NetworkBehaviour
{
    public BombSpawner bombSpawner;
    public NetworkVariable<int> buttonPressed = new NetworkVariable<int>(0);
    private NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsServer)
        {
            return;
        }

        if(buttonPressed.Value == networkManager.ConnectedClients.Count)
        {
            StartGame();
        }
    }

    public void ButtonPushedOn()
    {
        buttonPressed.Value++;
    }

    public void ButtonPushedOff()
    {
        buttonPressed.Value--;
    }


    private void StartGame()
    {
        bombSpawner.SpawnInstance();
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }

}
