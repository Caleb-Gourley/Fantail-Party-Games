using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTimer : MonoBehaviour
{

    [SerializeField] private PlayersInBombRangeTracker playerInBombRangeTracker;
    [SerializeField] private float minTime, maxTime;
    private float timerLength;
    // Start is called before the first frame update
    void Start()
    {
        timerLength = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        timerLength -= Time.deltaTime;
        if(timerLength <= 0)
        {
            Debug.Log("Exploded");
            Destroy(gameObject);
            if (playerInBombRangeTracker != null)
            {
                foreach(GameObject player in playerInBombRangeTracker.playersInRange)
                {
                    Debug.Log("Player eliminated " + player.name);
                }
            }
        }
    }
}