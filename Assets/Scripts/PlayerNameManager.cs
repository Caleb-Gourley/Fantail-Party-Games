using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameManager : NetworkBehaviour
{
    public static PlayerNameManager Instance { get; private set; }

    private string playerName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetPlayerData(string DisplayName)
    {
        playerName = DisplayName;
        Debug.Log("Player name set to: " + playerName);
    }
}
