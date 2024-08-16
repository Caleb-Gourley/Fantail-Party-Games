using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class RoundManager : MonoBehaviour
{
    [Header("Game Start")]
    // public GameObject startButton;
    public BalloonSpawn balloonSpawn; 
    public PopToStart popToStart;
    public float timer = 60f; 
    public float currentTimer = 60f; 

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
       // StartGame();
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
        currentTimer = timer;
        canvasGroup.alpha = 0;
        scoreManager.ResetScore();
        // Start a round couroutine and start spawning balloons if all players are ready 
        // StartCoroutine(RoundTimer());
        // balloonSpawn.StartSpawning(); 
        StartCoroutine(RoundTimer());

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
        BalloonManager[] balloonManagers = FindObjectsOfType<BalloonManager>();

        foreach (BalloonManager balloonManager in balloonManagers)
        {
            balloonSpawn.hasGameStarted = false;
           balloonManager.PopBalloon();
        }
        popToStart.gameObject.SetActive(true);
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
    public IEnumerator RoundTimer()
    {


        while (currentTimer > 0f)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            currentTimer -= 1f; // Decrease the time remaining by 1 second


            //Debug.Log("Time Remaining: " + currentTimer);
        }

        // Round is over, perform any necessary actions
        GameOver();
    }
}
