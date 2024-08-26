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
    private BombRoundManager bombRoundManager;
    public BombPlayersManager bombPlayersManager;
    public GameObject button;
    
    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        bombRoundManager = FindObjectOfType<BombRoundManager>();
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
            StartGameRpc();
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

    [Rpc(SendTo.Everyone)]
    private void StartGameRpc()
    {
        buttonPressed.Value = 0;
        bombSpawner.SpawnInstance();
        button.SetActive(false);
        foreach (var player in bombPlayersManager.GetPlayersList())
        {
            player.GetComponent<ScoreManager>().ResetAliveState();
            player.GetComponent<ScoreManager>().ResetScore();
        }
        bombRoundManager.StartGame();
    }

    [Rpc(SendTo.Everyone)]
    public void EndGameRpc()
    {
        bombSpawner.Stop();
        button.GetComponent<StartButton>().isPressed = false;
        button.GetComponent<StartButton>().isBeingPressed = false;
        button.SetActive(true);
    }
    
}
