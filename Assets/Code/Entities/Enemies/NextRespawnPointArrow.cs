using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRespawnPointArrow : MonoBehaviour
{
    private RespawnPoint nextRespawn;
    private RespawnPoint[] allRespawnPoints;
    public Transform aimPivot;
    private CheckpointFlag _checkpointFlag;
    

    private void Start()
    {
        
        allRespawnPoints = FindObjectsOfType<RespawnPoint>();
        _checkpointFlag = FindObjectOfType<CheckpointFlag>();
        FindClosestRespawnPoint();
        if (nextRespawn != null)
        {
            
            Vector3 direction = (nextRespawn.transform.position - transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aimPivot.rotation = Quaternion.Euler(0,0, angle);
 
            
        }
        else
        {
            Vector3 direction = (_checkpointFlag.transform.position - transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aimPivot.rotation = Quaternion.Euler(0,0, angle);
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
