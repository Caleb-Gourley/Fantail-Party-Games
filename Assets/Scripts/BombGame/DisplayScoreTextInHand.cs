using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScoreTextInHand : MonoBehaviour
{
    public GameObject player;
    public TMP_Text scoreText;
    private string scoreTextString;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreTextString = player.GetComponent<ScoreManager>().GetScore().ToString();
        scoreText.SetText(scoreTextString + "pts.");

        if (player.GetComponent<ScoreManager>().isAlive == false)
        {
            scoreText.SetText(scoreTextString + "pts. \n" + "You Are Dead");
        }
    }
}
