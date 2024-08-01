using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public void OnPlayerJoined()
    {
        // Could do a check here to see when a player is joined maybe in an update function or a function from Building Blocks
        // Then update score manager so the player has a score counter
        ScoreManager.Instance.SetPlayerCount(ScoreManager.Instance.playerCount + 1);
    }
}