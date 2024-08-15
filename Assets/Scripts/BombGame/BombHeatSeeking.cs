using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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





    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager");
        playersList = playerManager.GetComponent<BombPlayersManager>().GetPlayersList();
        rb = GetComponent<Rigidbody>();

        closestPlayer = playersList[UnityEngine.Random.Range(0, playersList.Count)];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(closestPlayer != null)
        {
            rb.velocity = transform.forward * speed;

            RotateBombTowardsPlayer();

            StopBeforePlayer();
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
        if(other.gameObject.name.Contains("Rigidbody"))
        {
            closestPlayer = null;
            StartCoroutine(WaitAndFindClosestPlayer());
            


            // transform.position = Vector3.MoveTowards(transform.position, closestPlayer.transform.position, Vector3.Magnitude(rb.velocity));
        }
    }

    IEnumerator WaitAndFindClosestPlayer()
    {
        
        yield return new WaitForSeconds(1.5f);
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
        this.closestPlayer = closestPlayer;;
    }
}
