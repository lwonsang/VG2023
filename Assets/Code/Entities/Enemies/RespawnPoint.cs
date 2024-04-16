using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class RespawnPoint : MonoBehaviour
{
    public float startTimeBtwSpawns;
    public float sTimeBtwSpawns;
    private float timeBtwSpawns;
    public GameObject enemy;
    private int enemiesSpawned;
    public int maxEnemiesNumber;
    private bool hasTrigged = false;
    public bool autoRespawn;
    private List<GameObject> listOfTanks;
    private bool allEnemiesDefeated = false;
    public GameObject nextRespawnPointArrowPrefab;
    public int enemiesAtATime = 5;
    private GameObject[] allTanks;
    public CharacterBase chari;
    public CharacterBase slime;

    // Define range for random spawn position
    public Vector2 spawnRange;

    // Start is called before the first frame update
    void Start()
    {
        sTimeBtwSpawns = startTimeBtwSpawns;
        timeBtwSpawns = startTimeBtwSpawns;
        enemiesSpawned = 0;
        listOfTanks = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        allTanks = GameObject.FindGameObjectsWithTag("Enemy_Hitbox");
        EnemySpawnSpeed();
        if (enemiesSpawned < maxEnemiesNumber && (hasTrigged || autoRespawn))
        {
            if (timeBtwSpawns <= 0)
            {
                // Generate random position within the spawn range
                Vector3 randomSpawnPosition = transform.position + new Vector3(Random.Range(-spawnRange.x, spawnRange.x), Random.Range(-spawnRange.y, spawnRange.y), 0.0f);
                listOfTanks.Add(Instantiate(enemy, randomSpawnPosition, Quaternion.identity));
                enemiesSpawned++;
                timeBtwSpawns = sTimeBtwSpawns;
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
                SoundManager.instance.PlayRespawnDefeated();
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

    void EnemySpawnSpeed()
    {
        if (chari.totalhealth > slime.totalhealth)
        {
            sTimeBtwSpawns = startTimeBtwSpawns*50f/chari.totalhealth;
        }
        else
        {
            sTimeBtwSpawns = startTimeBtwSpawns*60f/slime.totalhealth;
        }
        if (sTimeBtwSpawns < startTimeBtwSpawns/3)
        {
            sTimeBtwSpawns = startTimeBtwSpawns/3;
        }
    }
}