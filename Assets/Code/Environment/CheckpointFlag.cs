using System;
using System.Collections;
using System.Collections.Generic;
using Main;
using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    private RespawnPoint[] allRespawnPoints;
    private TankController[] allEnemies;
    public Animator _animator;
    AudioSource _AudioSource;
    public AudioClip checkpointFlagCleared;
    public AudioClip checkpointFlagMissed;
    private bool complete = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("CheckpointFlagWave");
        _AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        allRespawnPoints = FindObjectsOfType<RespawnPoint>();
        allEnemies = FindObjectsOfType<TankController>();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.parent.name == "Player" && allRespawnPoints.Length == 0 && allEnemies.Length == 0 &&
            complete == false) 
        {
            print("Reached checkpoint with all enemies defeated");
            _animator.SetTrigger("CheckpointClearedWave");
            _AudioSource.PlayOneShot(checkpointFlagCleared);
            complete = true;
        }
        else if (other.transform.parent.name == "Player" && complete == false)
        {
            print("All enemies not defeated");
            _AudioSource.PlayOneShot(checkpointFlagMissed);
        }
    }
}
