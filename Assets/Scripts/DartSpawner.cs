using Meta.XR.MultiplayerBlocks.NGO;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DartSpawner : MonoBehaviour
{
    public Transform headLocation;
    public FingerTracker fingerTracker;
    public float distanceToPinch = 0.03f;
    public GameObject dartModel;
    public GameObject throwableDart;
    public string hand;

    private Vector3 previousPosition;
    public Vector3[] velocityList;
    private int veclocityIndex;
    private Vector3 currentPosition;
    public Vector3 velocity;

    private float growDuration = 0.1f;
    private Vector3 maxDartSize = new Vector3(0.1f, 0.1f, 0.1f);
    public ParticleSystem particle;
    private bool spawned = false;


    public float checkRadius = 0.05f; 
    public float checkLength = 100.0f;

    public AutoMatchmakingNGO autoMatchmakingNGO;
    public bool hasGameStarted = false;

    private void Start()
    {
        if (!hasGameStarted && autoMatchmakingNGO != null)
        {
            StartCoroutine(Wait(autoMatchmakingNGO.maxRetries * autoMatchmakingNGO.retryInterval.y));
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasGameStarted)
        {
            if (hand == "left")
            {
                SpawnDartOnPinch(fingerTracker.left, dartModel);
            }
            else if (hand == "right")
            {
                SpawnDartOnPinch(fingerTracker.right, dartModel);
            }

            CalculateVelocity();
        }
    }

    private void SpawnDartOnPinch(Hand hand, GameObject dart)
    {
        if (fingerTracker.GetDistanceBetween(hand.index, hand.thumb) < distanceToPinch || fingerTracker.GetDistanceBetween(hand.middle, hand.thumb) < distanceToPinch)
        {
            dart.SetActive(true);
            if (!spawned)
            {
                StartCoroutine(DartGrow());
                spawned = true;

            }
        }
        else
        {
            if (dart.activeSelf)
            {
                SpawnDart();
            }

            dart.SetActive(false);
        }
    }

    private Vector3 CheckForBalloons(Vector3 velocityDirection)
    {
        Vector3 checkStart = transform.position;
        Vector3 checkEnd = transform.position + velocityDirection * checkLength;

        Collider[] hitColliders = Physics.OverlapCapsule(checkStart, checkEnd, checkRadius);

        List<GameObject> balloons = new List<GameObject>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Balloon"))
            {
                balloons.Add(collider.gameObject);
            }
        }

        Vector3 closestVector = Vector3.one;
        float closestAngle = 100;

        foreach (GameObject balloon in balloons)
        {
            Debug.Log("Balloon found");
            Vector3 direction = (balloon.transform.position - transform.position).normalized;
            float alignmentAngle = Vector3.Angle(velocityDirection, direction);
            if(alignmentAngle <= closestAngle)
            {
                Debug.Log(balloon.GetComponent<BalloonManager>().balloonTypeIndex);
                Debug.DrawRay(transform.position, direction * 100f, Color.magenta, 20f);
                closestAngle = alignmentAngle;
                closestVector = direction;
            }
        }

        return closestVector;
    }

    private void SpawnDart()
    {   
        dartModel.transform.localScale = Vector3.zero;
        particle.Stop();


        Vector3 infrontOfHead = headLocation.forward;
       // Debug.DrawRay(headLocation.position, infrontOfHead * 100f, Color.blue, 1f);
       // Debug.DrawRay(transform.position, velocity * 100f, Color.red, 1f);

        Vector3 newVelocity = velocity;


        if (Physics.Raycast(transform.position, infrontOfHead, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            float alignmentAngle = Vector3.Angle(velocity.normalized, direction);

           // Debug.DrawRay(transform.position, velocity.magnitude * direction * 100f, Color.yellow, 1f);
            if (alignmentAngle < 30)
            {
                
                Debug.DrawRay(transform.position, ((velocity.magnitude * direction * 2 + velocity) / 3) * 100f, Color.green, 1f);
                Vector3 closestVector = CheckForBalloons((velocity.magnitude * direction * 2 + velocity) / 3);
                newVelocity = (velocity.magnitude * direction * 1 + velocity + closestVector * 2) / 4;
                //newVelocity = closestVector;
               // Debug.Log("LockOn");
                Debug.DrawRay(transform.position, newVelocity * 100f, Color.black, 1f);
            }
            
        }

        GameObject newDart = Instantiate(throwableDart, transform.position, transform.rotation); 
        newDart.tag = "Dart"; 
        // Tag for collision detection and maybe later each camera rig could set a unique tag for each player darts by adding a dart 
        // data script to the dart prefab and setting the player number here - Luke
        //newDart.GetComponent<NetworkObject>().Spawn();
        newDart.GetComponent<Rigidbody>().isKinematic = false;
        newDart.GetComponent<Rigidbody>().velocity = newVelocity * 3;
        if (newDart.GetComponent<Rigidbody>().velocity.magnitude < 2)
        {
            newDart.GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            newDart.transform.rotation = Quaternion.LookRotation(newDart.GetComponent<Rigidbody>().velocity) * Quaternion.Euler(-90, 0, 0);
            newDart.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, newDart.GetComponent<Rigidbody>().velocity.magnitude * 100, 0);
        }
        Destroy(newDart, 5);
        spawned = false;
    }
    
    private void CalculateVelocity()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        if (currentVelocity != Vector3.zero)
        {

            velocityList[veclocityIndex] = currentVelocity;
            veclocityIndex = (veclocityIndex + 1) % 5;

            Vector3 averageVelocity = Vector3.zero;
            for (int i = 0; i < 5; i++)
            {
                averageVelocity += velocityList[i];
            }
            averageVelocity /= 5;
            velocity = averageVelocity;
        }
        previousPosition = transform.position;
    }
    

    IEnumerator DartGrow() //Particles as well
    {
        particle.Play();
        float elapsedTime = 0f;

        while (elapsedTime < growDuration)
        {
            dartModel.transform.localScale = Vector3.Lerp(Vector3.zero, maxDartSize, elapsedTime / growDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dartModel.transform.localScale = maxDartSize;
    }

    IEnumerator Wait(float waitAmount)
    {
        yield return new WaitForSeconds(waitAmount * 5);
        hasGameStarted = true;
    }
}
