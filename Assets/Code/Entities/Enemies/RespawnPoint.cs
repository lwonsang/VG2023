using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RespawnPoint : MonoBehaviour
{
    public float startTimeBtwSpawns;
    private float timeBtwSpawns;
    public GameObject enemy;
    public Transform target;
    private int enemiesSpawned;
    public int maxEnemiesNumber;
    private bool hasTrigged = false;
    public bool autoRespawn;

    // Define range for random spawn position
    public Vector2 spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
        enemiesSpawned = 0;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (hasTrigged || autoRespawn)
        {
            if (timeBtwSpawns <= 0)
            {
                // Generate random position within the spawn range
                Vector3 randomSpawnPosition = transform.position + new Vector3(Random.Range(-spawnRange.x, spawnRange.x), Random.Range(-spawnRange.y, spawnRange.y), 0.0f);
                Instantiate(enemy, randomSpawnPosition, Quaternion.identity);
                enemiesSpawned++;
                print(enemiesSpawned);
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.transform.parent != null)
        {
            if (other.transform.parent.gameObject.name == "Player" && enemiesSpawned < maxEnemiesNumber)
            {
                hasTrigged = true;
            }
        }
    }
}