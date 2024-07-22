using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand
{
    public Transform palm;
    public Transform thumb;
    public Transform index;
    public Transform middle;
    public Transform ring;
    public Transform pinky;
}

public class FingerTracker : MonoBehaviour
{

    [Header("Left Hand")]
    [SerializeField]
    public Hand left;

    [Header("Right Hand")]
    [SerializeField]
    public Hand right;

    public float GetDistanceBetween(Transform finger1, Transform finger2)
    {
        return Vector3.Distance(finger1.position, finger2.position);
    }
}
