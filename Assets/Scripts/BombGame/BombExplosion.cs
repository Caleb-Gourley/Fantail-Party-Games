using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private BombPlayersManager bombPlayersManager;
    public List<GameObject> playerList;
    private List<GameObject> playerNotHitByBomb = new List<GameObject>();
    public GameObject lastTouchedPlayer;
    public int numPlayersEliminated;
    private BombRoundManager bombRoundManager;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        bombPlayersManager = GetComponent<BombPlayersManager>();
        bombRoundManager = GetComponent<BombRoundManager>();
        playerList = bombPlayersManager.GetPlayersList();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerList == null)
        {
            GetActivePlayers();
        }

        if (CheckAllPlayersEliminated())
        {
            bombRoundManager.GameOver();
        }
    }

    private void GetActivePlayers()
    {
        playerList = bombPlayersManager.GetPlayersList();
    }

    public void SetLastTouchedPlayer(GameObject player)
    {
        lastTouchedPlayer = player;
        Debug.Log("Last Touch Player is " + lastTouchedPlayer);
    }

    public void OnBombExplosion(GameObject bomb)
    {
        audioSource.Play();

        foreach (GameObject player in playerList)
        {
            RaycastHit hit;
            Physics.Linecast(bomb.transform.position, player.transform.position, out hit);
            Debug.DrawLine(bomb.transform.position, player.transform.position, Color.red, 4.0f);

            if (hit.collider.tag == "Wall")
            {
                //playerNotHitByBomb.Add(player);
                Debug.Log("WALL HIT _______");
                player.GetComponent<SFXScript>().PlayClipReward();
            }
            else
            {
                if (player.GetComponent<ScoreManager>().isAlive)
                {
                    numPlayersEliminated++;
                }
                player.GetComponent<SFXScript>().PlayClipGG();

                player.GetComponent<ScoreManager>().StopScoring();
            }
        }

        Debug.Log("Num Players Eliminated: " + numPlayersEliminated);

        if (lastTouchedPlayer != null && lastTouchedPlayer.GetComponent<ScoreManager>().isAlive == false)
        {
            lastTouchedPlayer.GetComponent<ScoreManager>().AddBonusPoints(numPlayersEliminated * 10);
            Debug.Log(lastTouchedPlayer + " has eliminated " + numPlayersEliminated + " gaining score: " + numPlayersEliminated * 10);
        }

        numPlayersEliminated = 0;
        lastTouchedPlayer = null;
    }

    public bool CheckAllPlayersEliminated()
    {
        foreach (GameObject player in playerList)
        {
            if (player.GetComponent<ScoreManager>().isAlive)
            {
                return false;
            }
        }
        return true;
    }
}
