using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartDirectionAdjusment : MonoBehaviour
{
    public GameObject particle;
    public Transform particleSpawnLocation;

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity) * Quaternion.Euler(-90, 0, 0);
        transform.rotation = rotation;
    }

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


