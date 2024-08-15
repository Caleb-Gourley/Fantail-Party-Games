using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public float minTime, maxTime;
    private float timerLength;

    // Start is called before the first frame update
    void Start()
    {
        timerLength = Random.Range(minTime, maxTime);
        // Debug.Log("Wall Despawn Timer: " + timerLength + "seconds");

        Destroy(gameObject.transform.parent.gameObject, timerLength);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
