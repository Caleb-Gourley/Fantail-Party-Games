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

        if (popEffect != null) 
        {
            popEffect.transform.SetParent(null);
            popEffect.Play();
            Destroy(popEffect.gameObject, popEffect.main.duration);
        }       
        gameObject.SetActive(false); //Needs to hid mesh then wait to disable object aftfter effect
    }

}
