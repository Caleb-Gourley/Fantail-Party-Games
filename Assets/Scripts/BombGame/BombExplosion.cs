using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private BombPlayersManager bombPlayersManager;
    public List<GameObject> playerList;
    private List<GameObject> playerNotHitByBomb = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        bombPlayersManager = GetComponent<BombPlayersManager>();
        playerList = bombPlayersManager.GetPlayersList();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerList == null)
        {
            GetActivePlayers();
        }
    }

    private void GetActivePlayers()
    {
        playerList = bombPlayersManager.GetPlayersList();
    }

    public void OnBombExplosion(GameObject bomb)
    {
        foreach (GameObject player in playerList)
        {
            RaycastHit hit;
            Physics.Linecast(bomb.transform.position, player.transform.position, out hit);
            Debug.DrawLine(bomb.transform.position, player.transform.position, Color.red, 4.0f);

            if (hit.collider.tag == "Wall")
            {
                playerNotHitByBomb.Add(player);
            }
            else
            {
                player.GetComponent<ScoreManager>().StopScoring();
            }
        }
    }
}
