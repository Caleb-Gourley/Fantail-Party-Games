using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int highScore = 0;
    private bool isAlive = true;

    void Start()
    {
        ResetScore();
        StartCoroutine(UpdateScoreEverySecond());
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
        score += points;
        //Debug.Log($"Scored {points} points!");
        //Debug.Log($"{gameObject.name} Total score is now {score} points.");
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
        Debug.Log($"{gameObject.name} has stopped earning points.");
    }

    public void SetHighScore()
    {
        if (highScore < score)
        {
            highScore = score;
        }
    }
}