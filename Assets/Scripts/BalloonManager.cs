using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public ParticleSystem popEffect;
    public GameObject[] balloonModels;
    public Renderer[] balloonRendererTop;
    public Renderer[] balloonRendererBottom;
    public Material[] balloonColours;

    //[HideInInspector]
    public FindSpawnPositions findSpawnPositions;

    public int balloonTypeIndex;
    public int balloonColourIndex;
    public bool test;

    //Stats for growth over time
    public float timeToGrow = 0.5f;
    public Vector3 finalSize = new Vector3(0.3f, 0.3f, 0.3f);

    //Stats for upwards Rotation
    private float rotationSpeed = 1f; 
    private Quaternion targetRotation;

    void Start()
    {
        transform.parent = null;
        findSpawnPositions = FindObjectOfType<FindSpawnPositions>();
        //Gets a random balloon type and sets a random colour if it's not a bomb
        if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
        {
            balloonTypeIndex = 0;
        }
        else
        {
            balloonTypeIndex = UnityEngine.Random.Range(1, balloonModels.Length);
        }
        if(balloonTypeIndex != 0 )
        {
            balloonColourIndex = UnityEngine.Random.Range(0, balloonColours.Length);
            balloonRendererTop[balloonTypeIndex].material = balloonColours[balloonColourIndex];
            balloonRendererBottom[balloonTypeIndex].material = balloonColours[balloonColourIndex];
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
            --findSpawnPositions.ammountSpawned;
            Destroy(gameObject);
        }
    }

    private void FaceUp()
    {
        // Slowly makes balloons face up
        Quaternion upright = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        float angle = Mathf.Sin(Time.time * 5) * 0.1f;
        Quaternion currentRotation = Quaternion.AngleAxis(angle, transform.right);
        targetRotation = upright * currentRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator GrowObject()
    {
        float elapsedTime = 0f;

        Vector3 launchForce = (transform.up * UnityEngine.Random.Range(-0.01f, 0.01f)) + (transform.right * UnityEngine.Random.Range(-0.01f, 0.01f));
        while (elapsedTime < timeToGrow)
        {
            
            GetComponent<Rigidbody>().AddForce(launchForce, ForceMode.Impulse);
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
            --findSpawnPositions.ammountSpawned;
            Destroy(gameObject);

        }
    }
}


            