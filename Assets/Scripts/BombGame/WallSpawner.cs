using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public float minTime, maxTime;
    private float timerLength;
    public GameObject BuildingBlockFindWallLocationSpawner;

    // Start is called before the first frame update
    void Start()
    {
        timerLength = Random.Range(minTime, maxTime);
        Instantiate(BuildingBlockFindWallLocationSpawner);
        Instantiate(BuildingBlockFindWallLocationSpawner);
        Instantiate(BuildingBlockFindWallLocationSpawner);
        Instantiate(BuildingBlockFindWallLocationSpawner);
    }

    // Update is called once per frame
    void Update()
    {
        timerLength -= Time.deltaTime;
        if (timerLength <= 0)
        {
            timerLength = 100f;
            Instantiate(BuildingBlockFindWallLocationSpawner);
            timerLength = Random.Range(minTime, maxTime);
            // Debug.Log("WALL TIMERRRRRRRRRRRRRRRRR: " + timerLength);
        }
    }
}
