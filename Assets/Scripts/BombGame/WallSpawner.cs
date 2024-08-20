using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{

    public GameObject BuildingBlockFindWallLocationSpawner;
    public int numberOfWallsInScene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        numberOfWallsInScene = GameObject.FindGameObjectsWithTag("Wall").Length;

        if (numberOfWallsInScene < 4)
        {
            Instantiate(BuildingBlockFindWallLocationSpawner);
        }

    }


}
