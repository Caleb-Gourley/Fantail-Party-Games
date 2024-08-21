using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip impactSF;
    public AudioClip rewardSF;
    public AudioClip ggSF;

    private void Start()
    {
       audioSource = GetComponent<AudioSource>();
    }
    public void PlayClipImpact()
    {
        audioSource.PlayOneShot(impactSF);
    }

    public void PlayClipReward()
    { 
        audioSource.PlayOneShot(rewardSF);
    }

    public void PlayClipGG()
    {
        audioSource.PlayOneShot(ggSF);
    }


}
