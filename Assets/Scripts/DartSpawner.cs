using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DartSpawner : MonoBehaviour
{
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


    // Update is called once per frame
    void FixedUpdate()
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

    private void SpawnDartOnPinch(Hand hand, GameObject dart)
    {
        if(fingerTracker.GetDistanceBetween(hand.index, hand.thumb) < distanceToPinch || fingerTracker.GetDistanceBetween(hand.middle, hand.thumb) < distanceToPinch)
        {
            dart.SetActive(true);
        }
        else
        {
            if (dart.activeSelf)
            {
                GameObject newDart = Instantiate(throwableDart, transform.position, transform.rotation);
                newDart.GetComponent<Rigidbody>().isKinematic = false;
                newDart.GetComponent<Rigidbody>().velocity = velocity;
                if (newDart.GetComponent<Rigidbody>().velocity.magnitude < 1)
                {
                    newDart.GetComponent<Rigidbody>().useGravity = true;
                }
                else
                {
                    newDart.transform.rotation = Quaternion.LookRotation(newDart.GetComponent<Rigidbody>().velocity) * Quaternion.Euler(-90, 0, 0);
                    newDart.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, newDart.GetComponent<Rigidbody>().velocity.magnitude * 100, 0);
                }
            }

            dart.SetActive(false);
        }
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
}
