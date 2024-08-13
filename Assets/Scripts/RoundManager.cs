using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [Header("Game Start")]
    // public GameObject startButton;
    public BalloonSpawn balloonSpawn;

    [Header("Game Over")]
    public Canvas gameOverCanvas;
    public TextMeshProUGUI gameOverScore;
    public TextMeshProUGUI highScore;
    public float fadeDuration = 1f;


    [Header("Testing")]
    public bool test;
    
    private ScoreManager scoreManager;
    private int highScoreValue = 0;
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Get components
        scoreManager = GetComponent<ScoreManager>();
        canvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();

        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
        StartGame();
    }

    void Update()
    {
        if (test)
        {
            GameOver();
        }
    }

    public void StartGame() // Called when the game is started
    {
        scoreManager.ResetScore();
        // Start a round couroutine and start spawning balloons if all players are ready 
        // StartCoroutine(RoundTimer());
        // balloonSpawn.StartSpawning(); 

    }

    public void GameOver() // Called when the game is over
    {
        gameOverCanvas.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvas());

        gameOverScore.text = scoreManager.GetScore().ToString();
        
        if (scoreManager.GetScore() > highScoreValue)
        {
            highScoreValue = scoreManager.GetScore();
            highScore.text = highScoreValue.ToString();
        }
        else
        {
            highScore.text = highScoreValue.ToString();
        }

        // Stop spawning balloons and remove all excess balloons
        // balloonSpawn.StopSpawning();
        // Add an option to restart the game for all players over the network
    }
    private IEnumerator FadeInCanvas()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
