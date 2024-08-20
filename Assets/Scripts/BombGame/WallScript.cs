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
    }
}
