using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartDirectionAdjusment : MonoBehaviour
{
    public GameObject particle;
    public Transform particleSpawnLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoxCollider>() || other.GetComponent<MeshCollider>())
        {
            if (!other.CompareTag("Balloon"))
            {
                GameObject TempParticles = Instantiate(particle);
                TempParticles.transform.position = particleSpawnLocation.position;
                gameObject.SetActive(false);
                Destroy(TempParticles, 0.6f);
                Destroy(gameObject, 0.6f);
            }
        }
       
    }
}


