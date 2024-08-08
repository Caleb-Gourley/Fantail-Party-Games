using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public ParticleSystem popEffect;
    public GameObject[] balloonModels;

    [HideInInspector]
    public BalloonSpawn balloonSpawner;

    public int balloonTypeIndex;
    public int balloonColourIndex;
    public bool test;

    //Stats for growth over time
    public float timeToGrow = 0.5f;
    public Vector3 finalSize = new Vector3(0.3f, 0.3f, 0.3f);

    //Stats for upwards Rotation
    private float rotationSpeed = 6f; 
    private Quaternion targetRotation;

    void Start()
    {
        transform.parent = null;
        balloonSpawner = FindObjectOfType<BalloonSpawn>();
        //Gets a random balloon type and sets a random colour if it's not a bomb
        float randomIndex = UnityEngine.Random.Range(0f, 1f);   //5%  
        if (randomIndex <= 0.05f)
        {
            balloonTypeIndex = 0;                         
        }
        else if (randomIndex > 0.05f && randomIndex <= 0.15f)   //10%
        {
            balloonTypeIndex = 5;
        }
        else if(randomIndex > 0.15f && randomIndex <= 0.3f)    //15%
        {
            balloonTypeIndex = 4;
        }
        else if (randomIndex > 0.3f && randomIndex <= 0.50f)   //20%
        {
            balloonTypeIndex = 3;
        }
        else if (randomIndex > 0.50f && randomIndex <= 0.75f)   //25%
        {
            balloonTypeIndex = 2;
        }
        else //25
        {
            balloonTypeIndex = 1;
        }
        


            balloonModels[balloonTypeIndex].SetActive(true);

        //Tells network manager that it has spawned
        GetComponent<NetworkObject>().Spawn();
        

        //Set scale ready to grow
        transform.localScale = Vector3.zero; 
        StartCoroutine(GrowObject());
    }

    private void Update()
    {
        FaceUp();
        if (test)
        {
            --balloonSpawner.balloonsSpawned;
            Destroy(gameObject);
        }
    }

    private void FaceUp()
    {
        // Slowly makes balloons face up
        Quaternion upright = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        float angle = Mathf.Sin(Time.time * 100) * 0.1f;
        Quaternion currentRotation = Quaternion.AngleAxis(angle, transform.right);
        targetRotation = upright * currentRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator GrowObject()
    {
        float elapsedTime = 0f;

        Vector3 launchForce = (transform.up * UnityEngine.Random.Range(-1f, 1f)) + (transform.right * UnityEngine.Random.Range(-1f, 1f));
        GetComponent<Rigidbody>().AddForce(launchForce, ForceMode.Impulse);
        while (elapsedTime < timeToGrow)
        {
            
           // GetComponent<Rigidbody>().AddForce(launchForce, ForceMode.Impulse);
            transform.localScale = Vector3.Lerp(Vector3.zero, finalSize, elapsedTime / timeToGrow);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        transform.localScale = finalSize;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dart"))
        {
            if (popEffect != null) // Plays Benjis pop effect when balloon is popped 
            {
                popEffect.transform.SetParent(null); 
                popEffect.Play();
                Destroy(popEffect.gameObject, popEffect.main.duration);
            }
            
            

            ScoreManager.Instance.AddScore(1, 100);
            // Add score to player 1 for testing 
            // When balloon is popped need to add a check here for which player popped the balloon 
            // Could do this by adding a script onto the dart that species which player it belongs to
            // DartData  dartData = collision.gameObject.GetComponent<DartData>();
            // ScoreManager.Instance.AddScore(dartData.playerNumber, 100);
            // This is just an example because it still means we have to set a player number from the dart spawner script somehow - Luke
            //--findSpawnPositions.ammountSpawned;
            --balloonSpawner.balloonsSpawned;
            Destroy(gameObject);

        }
    }
}


            