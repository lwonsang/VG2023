using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    private RespawnPoint[] allRespawnPoints;
    public Animator _animator;
    public Animation animation;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("CheckpointFlagWave");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        allRespawnPoints = FindObjectsOfType<RespawnPoint>();
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("fdas");
        if (other.transform.parent.name == "Player" && allRespawnPoints.Length==0)
        {
            print("Reached checkpoint with all enemies defeated");
            // _animator.SetTrigger("CheckpointClearedWave");
        }
        else if(allRespawnPoints.Length!=0)
        {
            print("respawnpoints failed");
            print(allRespawnPoints);
        }
        else if (other.transform.parent.name != "Player")
        {
            print(other.transform.parent.name);
            print("playertag failed");
        }
    }
}
