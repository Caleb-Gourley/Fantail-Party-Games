using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int playerCount = 1; // Default for testing purposes
    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    void Awake()
    {
        // Check to see if instance exists when a new player joins and does not create a new instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {

        InitializeScores();
    }

    void InitializeScores()
    {
        // Initialize scores for each player
        for (int i = 1; i <= playerCount; i++)
        {
            playerScores[i] = 0;
        }
    }

    public void AddScore(int playerNumber, int points)
    {
        if (playerScores.ContainsKey(playerNumber))
        {
            playerScores[playerNumber] += points;
            Debug.Log($"Player {playerNumber} scored {points} points!");
            Debug.Log($"Player {playerNumber} has {playerScores[playerNumber]} points in total.");
        }
        else
        {
            Debug.LogWarning($"Player {playerNumber} does not exist.");
        }
    }

    public void SetScore(int playerNumber, int score)
    {
        if (playerScores.ContainsKey(playerNumber))
        {
            playerScores[playerNumber] = score;
        }
        else
        {
            Debug.LogWarning($"Player {playerNumber} does not exist.");
        }
    }

    public int GetScore(int playerNumber)
    {
        if (playerScores.ContainsKey(playerNumber))
        {
            return playerScores[playerNumber];
        }
        else
        {
            Debug.LogWarning($"Player {playerNumber} does not exist.");
            return 0;
        }
    }

    public void ResetScores()
    {
        foreach (int playerNumber in playerScores.Keys)
        {
            playerScores[playerNumber] = 0;
        }
    }

    public void SetPlayerCount(int count)
    {
        playerCount = count;
        InitializeScores();
    }
}