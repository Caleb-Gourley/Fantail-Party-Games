using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PopToStart : MonoBehaviour
{
    public ParticleSystem popEffect;
    public RoundManager roundManager;
    public BalloonSpawn balloonSpawn;

    public bool test;

    public AudioClip[] soundEffects;

    private void Update()
    {
        if(test)
        {
            test = false;
            balloonSpawn.hasGameStarted = true;
            roundManager.StartGame();
            PopBalloon();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dart"))
        {
            balloonSpawn.hasGameStarted = true;
            roundManager.StartGame();
            PopBalloon();  
        }
    }

    public void PopBalloon()
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
        gameObject.SetActive(false); //Needs to hid mesh then wait to disable object aftfter effect
    }

    public void PlaySFX(GameObject balloonParticles)
    {
        int randomIndex = Random.Range(0, soundEffects.Length);
        AudioClip clipToPlay = soundEffects[randomIndex];
        AudioSource sfxSource = balloonParticles.AddComponent<AudioSource>();
        sfxSource.clip = clipToPlay;
        sfxSource.Play(0);
    }

}
