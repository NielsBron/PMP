using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public bool isPaused = false;

    void Update()
    {

    }
    
    public void SpawnPlayer()
    {
        Debug.Log("Player Spawned");
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }
    
    public void DestroyPlayer()
    {
        Destroy(playerPrefab);
        Debug.Log("Player Destroyed");
    }   
}
