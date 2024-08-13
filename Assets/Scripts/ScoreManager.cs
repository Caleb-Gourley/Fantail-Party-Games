using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;

    void Start()
    {
        ResetScore();
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Scored {points} points!");
        Debug.Log($"Total score is now {score} points.");
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        Debug.Log($"Score set to {score} points.");
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        Debug.Log("Score reset to 0.");
    }
}