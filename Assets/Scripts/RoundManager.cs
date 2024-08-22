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
    public Canvas startCanvas;
    public GameObject CameraRig;

    [Header("Game Over")]
    public Canvas gameOverCanvas;
    public TextMeshProUGUI gameOverScore;
    public TextMeshProUGUI highScore;
    public float fadeDuration = 1f;


    [Header("Testing")]
    public bool test;

    [Header("UI Positioning")]
    public float distanceFromCamera = 2f;
    public float popToStartOffset = 0.5f;
    
    private ScoreManager scoreManager;
    private int highScoreValue = 0;
    private CanvasGroup gameOverCG;
    private CanvasGroup startGameCG;


    void Start()
    {
        // Get components
        scoreManager = GetComponent<ScoreManager>();
        gameOverCG = gameOverCanvas.GetComponent<CanvasGroup>();
        startGameCG = startCanvas.GetComponent<CanvasGroup>();
        startCanvas.gameObject.SetActive(true);
        startGameCG.alpha = 0f;
        StartCoroutine(GameLoad());
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
        startCanvas.gameObject.SetActive(false);
        startGameCG.alpha = 0f;
        gameOverCG.alpha = 0f;
        scoreManager.ResetScore();
        // Start a round couroutine and start spawning balloons if all players are ready 
        // balloonSpawn.StartSpawning(); 
        StartCoroutine(RoundTimer());

    }

    public void GameOver() // Called when the game is over
    {
        gameOverCanvas.gameObject.SetActive(true);
        PositionCanvas(gameOverCanvas.gameObject);
        StartCoroutine(FadeInCanvas(gameOverCG));
        PositionPopToStart(popToStart.gameObject);
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

    }
    private IEnumerator FadeInCanvas(CanvasGroup canvasGroup)
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
    private void PositionCanvas(GameObject canvas)
    {
        if (CameraRig != null)
        {
            Vector3 cameraForward = CameraRig.transform.forward;
            Vector3 canvasPosition = CameraRig.transform.position + cameraForward * distanceFromCamera;
            canvas.transform.position = canvasPosition;
            canvas.transform.rotation = Quaternion.LookRotation(cameraForward);
            StartCoroutine(FadeInCanvas(canvas.GetComponent<CanvasGroup>()));
        }
    }

    private void PositionPopToStart(GameObject Balloon)
    {
        if (CameraRig != null && popToStart != null)
        {
            Vector3 cameraForward = CameraRig.transform.forward;
            Vector3 cameraUp = CameraRig.transform.up;
            Vector3 popToStartPosition = CameraRig.transform.position + (cameraUp * popToStartOffset) + (cameraForward * distanceFromCamera);
            Balloon.transform.position = popToStartPosition;
            Balloon.transform.rotation = Quaternion.LookRotation(cameraForward);
            Balloon.SetActive(true);
        }
    }
    public IEnumerator RoundTimer()
    {


        while (currentTimer > 0f)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            currentTimer -= 1f; // Decrease the time remaining by 1 second


            //Debug.Log("Time Remaining: " + currentTimer);
        }
        GameOver();
    }
    public IEnumerator GameLoad()
    {
        yield return new WaitForSeconds(3f);
        PositionCanvas(startCanvas.gameObject);
        PositionPopToStart(popToStart.gameObject);
    }
}
