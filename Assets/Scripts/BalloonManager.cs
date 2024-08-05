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

    public int balloonTypeIndex;
    public int balloonColourIndex;

    void Start()
    {
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
            balloonRendererTop[balloonColourIndex].material = balloonColours[balloonColourIndex];
            balloonRendererBottom[balloonColourIndex].material = balloonColours[balloonColourIndex];
        }
        balloonModels[balloonTypeIndex].SetActive(true);

        //Tells network manager that it has spawned
        GetComponent<NetworkObject>().Spawn();

        //Launches Balloon (Not working)
        Vector3 launchForce = (transform.up * UnityEngine.Random.Range(-1f, 1f)) + (transform.right * UnityEngine.Random.Range(-1f, 1f));
        GetComponent<Rigidbody>().AddForce(launchForce, ForceMode.Impulse);
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
            Destroy(gameObject);

        }
    }
}


            