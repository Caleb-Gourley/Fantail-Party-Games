using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopRando : MonoBehaviour
{
    
    [SerializeField] public AudioClip[] soundEffects = default;
    // Start is called before the first frame update
    public void PlaySFX(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        AudioClip clipToPlay = clips[randomIndex];
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = clipToPlay;
        sfxSource.loop = false;
        sfxSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
