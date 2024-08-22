using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public float minTime, maxTime;
    public float timerLength;
    public Color startColor = Color.red;
    public Color endColor = Color.white;
    public float blinkSpeed;
    public bool sfxPlayed;

    Renderer cubeRen;

    void Start()
    {
        cubeRen = transform.Find("Cube").GetComponent<Renderer>();
        blinkSpeed = 8;


        timerLength = Random.Range(minTime, maxTime);
        // Debug.Log("Wall Despawn Timer: " + timerLength + "seconds");

        Destroy(gameObject.transform.parent.gameObject, timerLength);
    }

    // Update is called once per frame
    void Update()
    {
        timerLength -= Time.deltaTime;
        if (timerLength <= 3)
        {
            cubeRen.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * blinkSpeed, 1));
        }

        if (timerLength <= 0.5f && !sfxPlayed)
        {
            Debug.Log("ShieldSFX Insert here");
            PlayAudioSFX();
        }
    }


    void PlayAudioSFX()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        sfxPlayed = true;   
       
    }

    [ContextMenu("play aduio")]
    void temp()
    {
        PlayAudioSFX();
    }
}
