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
    private StartButton startButton;
    
    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        bombRoundManager = FindObjectOfType<BombRoundManager>();
        startButton = FindObjectOfType<StartButton>();
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
            bombRoundManager.StartGame();
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
        buttonPressed.Value = 0;
        bombSpawner.SpawnInstance();
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            MeshRenderer[] meshRenderers = button.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
        }
    }

    public void EndGame()
    {
        bombSpawner.Destroy();
        startButton.isPressed = false;
        startButton.isBeingPressed = false;
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            MeshRenderer[] meshRenderers = button.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = true;
            }
        }
    }
    
}
