using Meta.WitAi.Attributes;
using Meta.XR.MultiplayerBlocks.NGO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static FindSpawnPositions;
//using static UnityEditor.Progress;

public class BalloonSpawn : MonoBehaviour
{
    public AutoMatchmakingNGO autoMatchmakingNGO;
    public FindSpawnPositions spawnPositions;
    public bool hasGameStarted = true;  // For testing purposes game starts immediately
    public int maxAmount = 30;
    public float spawnRate = 0.5f;
    [HideInInspector]
    public int balloonsSpawned = 0;

    private float lastTime;

    private void Start()
    {
        if (!hasGameStarted && autoMatchmakingNGO != null)
        {
            StartCoroutine(Wait(autoMatchmakingNGO.maxRetries * autoMatchmakingNGO.retryInterval.y));
        }
    }

    public void FixedUpdate()
    {
        if (hasGameStarted)
        {
            lastTime += Time.fixedDeltaTime;
            if (lastTime >= spawnRate)
            {
                lastTime = 0;
                if (balloonsSpawned < maxAmount)
                {
                    spawnPositions.StartSpawn();
                    ++balloonsSpawned;
                }
            }
        }
    }

    IEnumerator Wait(float waitAmount)
    {
        yield return new WaitForSeconds(waitAmount*5);
        hasGameStarted = true;
    }

    //private void Start()
    //{
    //    if (hasGameStarted)  // Can change this later so game starts when users press a button or something
    //    {
    //        StartCoroutine(BalloonCycle());
    //    }
    //}

    //private IEnumerator BalloonCycle()
    //{
    //    while (true)  // Can also change this to a variable so ballons stop spawning when game ends
    //    {
    //        yield return StartCoroutine(GrowAndLaunchBalloon());
    //        yield return new WaitForSeconds(waitTime);
    //    }
    //}

    //private IEnumerator GrowAndLaunchBalloon()
    //{
    //    // Gets the spawn position of the balloon spawns it then gets it rigidbody for launching
    //    Vector3 spawnPosition = transform.position + Vector3.up * launchOffset;
    //    GameObject balloon = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
    //    Rigidbody rb = balloon.GetComponent<Rigidbody>();


    //    rb.isKinematic = false; // Physics not work if kinematic is true
    //    yield return new WaitForFixedUpdate(); // Wait for physics to update


    //    // ================ Ballloon growing effect ====================
    //    Vector3 originalScale = balloon.transform.localScale;
    //    balloon.transform.localScale = Vector3.zero;

    //    float elapsedTime = 0f;
    //    while (elapsedTime < growthTime)
    //    {
    //        balloon.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, elapsedTime / growthTime);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    balloon.transform.localScale = originalScale;

    //    // =============== Launch balloon effect ===================
    //    if (Vector3.Distance(balloon.transform.localScale, originalScale) < 0.01f) // Check to make sure ballon fully grew
    //    {
    //        yield return new WaitForFixedUpdate();

    //        // Picks random direction to shoot balloon and calculates force
    //        float randomAngle = Random.Range(0f, 360f);
    //        Vector3 horizontalDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;
    //        Vector3 launchForce = (Vector3.up * verticalLaunchForce) + (horizontalDirection * horizontalLaunchForce);

    //        // Launches balloon
    //        rb.AddForce(launchForce, ForceMode.Impulse);
    //        Debug.Log($"Launched balloon with force: {launchForce}");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Balloon did not fully grow before launch attempt.");
    //    }
    //}
}