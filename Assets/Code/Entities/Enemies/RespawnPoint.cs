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
    private int enemiesSpawned;
    public int maxEnemiesNumber;
    private bool hasTrigged = false;
    public bool autoRespawn;
    private List<GameObject> listOfTanks;
    private bool allEnemiesDefeated = false;
    public GameObject nextRespawnPointArrowPrefab;

    // Define range for random spawn position
    public Vector2 spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
        enemiesSpawned = 0;
        listOfTanks = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (enemiesSpawned < maxEnemiesNumber && (hasTrigged || autoRespawn))
        {
            if (timeBtwSpawns <= 0)
            {
                // Generate random position within the spawn range
                Vector3 randomSpawnPosition = transform.position + new Vector3(Random.Range(-spawnRange.x, spawnRange.x), Random.Range(-spawnRange.y, spawnRange.y), 0.0f);
                listOfTanks.Add(Instantiate(enemy, randomSpawnPosition, Quaternion.identity));
                enemiesSpawned++;
                timeBtwSpawns = startTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }

        if (enemiesSpawned >= maxEnemiesNumber && !allEnemiesDefeated)
        {
            allEnemiesDefeated = AllTanksDestroyedCheck();
            if (allEnemiesDefeated)
            {
                Vector3 arrowPosition = transform.position + new Vector3(0, 1.25f, 0);
                Instantiate(nextRespawnPointArrowPrefab, arrowPosition, Quaternion.identity);
                Destroy(gameObject);
            }
            

        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.transform.parent != null)
        {
            if (other.transform.parent.gameObject.name == "Player")
            {
                hasTrigged = true;
            }
        }
    }

    bool AllTanksDestroyedCheck()
    {
        for (int i = 0; i < enemiesSpawned; ++i)
        {
            if (listOfTanks[i] != null)
            {
                return false;
            }
        }

        return true;
    }
}