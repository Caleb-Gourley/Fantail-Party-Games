using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;


public class BalloonManager : NetworkBehaviour
{
    public ParticleSystem popEffect;
    public ParticleSystem bombEffect;
    public GameObject[] balloonModels;

    [HideInInspector]
    public BalloonSpawn balloonSpawner;
    public ScoreManager ScoreManager;

    public int balloonTypeIndex;
    public int balloonColourIndex;
    public bool test;

    //Stats for growth over time
    public float timeToGrow = 0.5f;
    public Vector3 finalSize = new Vector3(0.3f, 0.3f, 0.3f);

    //Stats for upwards Rotation
    private float rotationSpeed = 6f; 
    private Quaternion targetRotation;

    [HideInInspector]
    public bool Spawned;
    public AudioClip[] soundEffects;

    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = new Vector3(-1000, -1000, -1000);
        transform.parent = null;
        balloonSpawner = FindObjectOfType<BalloonSpawn>();
        ScoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        FaceUp();
        if (test)
        {
            if (balloonTypeIndex == 0) // Bomb Balloon
            {
                Explode();
            }
        }
    }

    public void Inflate()
    {
        Spawned = true;
        GetComponent<Rigidbody>().isKinematic = false;
        InflateRpc();


        //Set scale ready to grow
        //transform.localScale = Vector3.zero;
        // StartCoroutine(GrowObject());
    }
    [Rpc(SendTo.Everyone)]
    private void InflateRpc()
    {
        //Gets a random balloon type and sets a random colour if it's not a bomb

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        ++balloonSpawner.balloonsSpawned;
        float randomIndex = UnityEngine.Random.Range(0f, 1f);   //5%  
        if (randomIndex <= 0.05f)
        {
            balloonTypeIndex = 0;
        }
        else if (randomIndex > 0.05f && randomIndex <= 0.15f)   //10%
        {
            balloonTypeIndex = 5;
        }
        else if (randomIndex > 0.15f && randomIndex <= 0.3f)    //15%
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


        ChangeBalloons();


    }

    void ChangeBalloons()
    {
        foreach (GameObject balloonModel in balloonModels)
        {
            if (balloonModel == balloonModels[balloonTypeIndex])
            {
                balloonModels[balloonTypeIndex].SetActive(true);
            }
            else
            {
                balloonModel.SetActive(false);
            }
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
            //Debug.Log("hit");
            if (balloonTypeIndex == 0) // Bomb Balloon
            {
                Explode();
            }
            else
            {
                PopBalloon();
            }
        }
    }

    public void PopBalloon()
    {
        int score = 0;
        switch (balloonTypeIndex)
        {
            case 1: score = 100; break;
            case 2: score = 200; break;
            case 3: score = 300; break;
            case 4: score = 400; break;
            case 5: score = 500; break;
            case 0: score = 0; break; // Bomb Balloon could maybe score negative points or something
        }
        ScoreManager.AddScore(score);
        PopBalloonRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void PopBalloonRpc()
    {
        if (balloonTypeIndex == 0) // Bomb Balloon
        {
            if (bombEffect != null) // Plays bomb effect when balloon is popped
            {
                ParticleSystem tempBombEffect = Instantiate(bombEffect, transform);
                PlaySFX(tempBombEffect.gameObject);
                tempBombEffect.transform.localPosition = Vector3.zero;
                tempBombEffect.transform.SetParent(null);
                tempBombEffect.Play();
                Destroy(tempBombEffect.gameObject, 5);
            }
        }
        else
        {
            if (popEffect != null) // Plays confetti effect when balloon is popped 
            {
                ParticleSystem tempPopEffect = Instantiate(popEffect, transform);
                PlaySFX(tempPopEffect.gameObject);
                tempPopEffect.transform.localPosition = Vector3.zero;
                tempPopEffect.transform.SetParent(null);
                tempPopEffect.Play();
                Destroy(tempPopEffect.gameObject, 5);
            }
        }
        --balloonSpawner.balloonsSpawned;
        Spawned = false;
        transform.position = new Vector3(-1000, -1000, -1000);
        GetComponent<Rigidbody>().isKinematic = true;
        balloonModels[balloonTypeIndex].SetActive(false);
    }

    public void PlaySFX(GameObject balloonParticles)
    {
        int randomIndex = Random.Range(0, soundEffects.Length);
        AudioClip clipToPlay = soundEffects[randomIndex];
        AudioSource sfxSource = balloonParticles.AddComponent<AudioSource>();
        sfxSource.clip = clipToPlay;
        sfxSource.Play(0);
    }

    void Explode()
    {

        Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var hitObjects in surroundingObjects)
        {
            BalloonManager balloon = hitObjects.GetComponent<BalloonManager>();
            if (balloon != null && balloon != this)
            {
                balloon.PopBalloon();
            }
        }
        PopBalloon();
    }
}    
     
     
            