using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandColliderScript : MonoBehaviour
{
    public GameObject playerCollider;
    public GameObject playerManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bomb"))
        {
            Debug.Log("BOMB HANDLED");
            playerCollider.GetComponent<ScoreManager>().AddScore(15);

            playerManager.GetComponent<BombExplosion>().SetLastTouchedPlayer(playerCollider);
        }
    }
}
