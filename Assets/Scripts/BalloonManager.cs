using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public ParticleSystem popEffect; 

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


            