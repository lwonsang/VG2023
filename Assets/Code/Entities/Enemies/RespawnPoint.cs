using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public float startTimeBtwSpawns;
    private float timeBtwSpawns;
    public GameObject enemy;
    public Transform target;
    private int enemiesSpawned;
    public int maxEnemiesNumber;

    // Define range for random spawn position
    public Vector2 spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;
        enemiesSpawned = 0;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target && enemiesSpawned < maxEnemiesNumber)
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
}