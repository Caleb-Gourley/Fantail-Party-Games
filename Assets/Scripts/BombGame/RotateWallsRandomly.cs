using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWallsRandomly : MonoBehaviour
{
    public Transform wallSpawner;
    private BombRoundManager bombRoundManager;

    void Start()

    {
        bombRoundManager = FindObjectOfType<BombRoundManager>();

        if (bombRoundManager.isGameActive)
        {
            foreach (Transform child in wallSpawner)
            {
                //Debug.Log("A wall has been rotated" + child.name);
                int randomRotationValue = Random.Range(0, 359);
                child.transform.rotation = Quaternion.Euler(0, randomRotationValue, 0);


            }
        }
    }
}
