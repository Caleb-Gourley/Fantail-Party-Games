using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartDirectionAdjusment : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoxCollider>() || other.GetComponent<MeshCollider>())
        {
            Destroy(gameObject);
        }
       
    }
}
