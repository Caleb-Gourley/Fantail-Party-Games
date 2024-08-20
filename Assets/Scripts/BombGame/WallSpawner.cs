using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{

    public GameObject BuildingBlockFindWallLocationSpawner;
    public int numberOfWallsInScene;
    private BombRoundManager bombRoundManager;

    void Start()
    {
        bombRoundManager = FindObjectOfType<BombRoundManager>();
    }

    void Update()
    {
        numberOfWallsInScene = GameObject.FindGameObjectsWithTag("Wall").Length;

        if (numberOfWallsInScene < 4 && bombRoundManager.isGameActive)
        {
            Instantiate(BuildingBlockFindWallLocationSpawner);
        }

    }


}
