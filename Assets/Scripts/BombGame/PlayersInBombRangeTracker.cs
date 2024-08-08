using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInBombRangeTracker : MonoBehaviour
{
    public List<GameObject> playersInRange;

    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            playersInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playersInRange.Remove(other.gameObject);
        }
    }
}

