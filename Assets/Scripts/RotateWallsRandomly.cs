using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWallsRandomly : MonoBehaviour
{
    public Transform wallSpawner;


    void Start()

    {
        foreach (Transform child in wallSpawner)
        {

            Debug.Log("LOOOOOK HEEERRREEEEEE" + child.name);
            int randomRotationValue = Random.Range(0, 359);
            child.transform.rotation = Quaternion.Euler(0, randomRotationValue, 0);
        }
    }


}
