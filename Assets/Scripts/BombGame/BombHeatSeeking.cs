using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BombHeatSeeking : MonoBehaviour
{
    private GameObject closestPlayer;
    private GameObject playerManager;
    private List<GameObject> playersList;
    private Rigidbody rb;
    public float speed;
    [SerializeField] private float rotateSpeed;
    private bool waiting = false;





    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("GameManager");
        playersList = playerManager.GetComponent<BombPlayersManager>().GetPlayersList();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playersList == null)
        {
            playersList = playerManager.GetComponent<BombPlayersManager>().GetPlayersList();
        }

        if(closestPlayer != null)
        {
            rb.velocity = transform.forward * speed;

            RotateBombTowardsPlayer();

            StopBeforePlayer();
        }
        else if(!waiting)
        {
            FindNewPlayer();
        }
    }

    private void StopBeforePlayer()
    {
        if(Vector3.Distance(transform.position, closestPlayer.transform.position) < .7f)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void RotateBombTowardsPlayer()
    {
        Vector3 heading = closestPlayer.gameObject.GetComponent<Rigidbody>().position + closestPlayer.gameObject.GetComponent<Rigidbody>().velocity;
        Quaternion rotation = Quaternion.LookRotation(heading - transform.position);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed));
    }

    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Collided with: " + other.gameObject.name);
        if(other.gameObject.name.Contains("Hand"))
        {   
            NetworkObject otherNetworkObject = FindNetworkObject(other.gameObject);
            GetComponent<NetworkObject>().ChangeOwnership(otherNetworkObject.OwnerClientId);
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            StopCoroutine(WaitAndFindClosestPlayer());
            closestPlayer = null;
            rb.AddForce(otherRb.velocity.normalized * 50, ForceMode.VelocityChange);
            StartCoroutine(WaitAndFindClosestPlayer());
            
            // transform.position = Vector3.MoveTowards(transform.position, closestPlayer.transform.position, Vector3.Magnitude(rb.velocity));
        }
    }

    private NetworkObject FindNetworkObject(GameObject other)
    {
        if(other.GetComponentInParent<NetworkObject>() == null)
        {
            return FindNetworkObject(other.transform.parent.gameObject); 
        }
        else
        {
            return other.GetComponentInParent<NetworkObject>();
        }
    }

    public void FindNewPlayer()
    {
        GameObject closestPlayer = null;
        float lowestAngle = 180;
        foreach (GameObject player in playersList)
        {
            float angle = Vector3.Angle(rb.velocity, player.transform.position);
            if (angle < lowestAngle && Vector3.Distance(transform.position, player.transform.position) <= 1)
            {
                closestPlayer = player;
                lowestAngle = angle;
            }
        }
        this.closestPlayer = closestPlayer;
    }

    IEnumerator WaitAndFindClosestPlayer()
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(1.5f);
        FindNewPlayer();
        waiting = false;
    }
}
