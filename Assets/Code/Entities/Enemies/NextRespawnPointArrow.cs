using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRespawnPointArrow : MonoBehaviour
{
    private RespawnPoint nextRespawn;
    private RespawnPoint[] allRespawnPoints;
    public Transform aimPivot;
    

    private void Start()
    {
        
        allRespawnPoints = FindObjectsOfType<RespawnPoint>();
        FindClosestRespawnPoint();
        if (nextRespawn != null)
        {
            
            Vector3 direction = (nextRespawn.transform.position - transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aimPivot.rotation = Quaternion.Euler(0,0, angle);
 
            
        }
        else
        {
            print("all enemies defeated");
        }
    }


    
    private void FindClosestRespawnPoint()
    {
        float closestDistance = Mathf.Infinity;
        foreach (RespawnPoint respawnPoint in allRespawnPoints)
        {
            float distance = Vector3.Distance(transform.position, respawnPoint.transform.position);
            if (distance < closestDistance & distance > 10)
            {
                closestDistance = distance;
                nextRespawn = respawnPoint;
            }
        }
    }
}
