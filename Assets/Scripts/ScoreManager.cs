using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    private int highScore = 0;
    public bool isAlive = true;
    private BombRoundManager bombRoundManager;

    void Start()
    {
        ResetScore();
        bombRoundManager = FindObjectOfType<BombRoundManager>();
    }

    void Update()
    {
        if (bombRoundManager.isGameActive)
        {
            StartCoroutine(UpdateScoreEverySecond());
        }
    }

    private IEnumerator UpdateScoreEverySecond()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(1);

            AddScore(1);
        }
    }

    public void AddScore(int points)
    {
        if (isAlive) 
        {
            score += points;
            //Debug.Log($"Scored {points} points!");
        }


        Debug.LogWarning($"{gameObject.name} Total score is now {score} points.");
    }

    public void SetScore(int newScore)
    {
        score = newScore;
       // Debug.Log($"Score set to {score} points.");
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

    public void StopScoring()
    {
        isAlive = false;
        Debug.LogWarning($"{gameObject.name} has stopped earning points.");
    }

    public void SetHighScore()
    {
        if (highScore < score)
        {
            highScore = score;
        }
    }

    public void ResetAliveState()
    {
        isAlive = true;
    }
}