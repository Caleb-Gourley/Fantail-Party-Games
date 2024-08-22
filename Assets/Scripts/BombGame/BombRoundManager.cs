using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class BombRoundManager : MonoBehaviour
{
    [Header("Game Over")] 
    public Canvas gameOverCanvas;
    public TextMeshProUGUI gameOverScore;
    public TextMeshProUGUI highScore;
    public float fadeDuration = 1.0f;

    private ScoreManager scoreManager;
    private int highScoreValue = 0;
    private CanvasGroup canvasGroup;

    private PushToStart pushToStart;
    public bool isGameActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        pushToStart = GetComponent<PushToStart>();
        scoreManager = FindObjectOfType<ScoreManager>();
        canvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();

        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        gameOverCanvas.gameObject.SetActive(false);
        canvasGroup.alpha = 0;
        scoreManager.ResetScore();
        scoreManager.ResetAliveState();
        isGameActive = true;
    }

    public void GameOver()
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

        pushToStart.EndGame();
        isGameActive = false;
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
