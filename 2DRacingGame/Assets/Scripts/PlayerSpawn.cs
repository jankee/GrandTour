using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
    public Transform playerSpawn;
    public Vector3 currentTrackPosition;

    public bool activeRespawnTimer          = false;
    public float respawnTimer               = 1f;
    public float resetRespawnTimer          = 1f;


    // Use this for initialization
    void Start()
    {
        if (playerSpawn != null)
        {
            transform.position = playerSpawn.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeRespawnTimer)
        {
            respawnTimer -= Time.deltaTime;
        }
    }
}
