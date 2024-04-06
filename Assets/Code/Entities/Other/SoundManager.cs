using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource audioSource;
    public AudioClip[] allSounds;
    public float audioVolume = 0.6f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = audioVolume;
    }

    public void PlaySoundChar1LeftClick(int attackNum)
    {
        if (attackNum == 3)
        {
            audioSource.pitch = 0.75f;
            audioSource.PlayOneShot(allSounds[0]);
        }
        else
        {
            audioSource.pitch = 1;
            audioSource.PlayOneShot(allSounds[0]);
        }
        
    }

    public void PlaySoundLevelUp()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(allSounds[1]);
    }

    public void PlaySoundGameOver()
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(allSounds[3]);
    }

    public void PlaySoundChar1RightClick()
    {
        audioSource.pitch = 1f;
        audioSource.clip = allSounds[2];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySoundChar2LeftClick()
    {
        audioSource.pitch = 1f;
        audioSource.volume /= 2f;
        audioSource.clip = allSounds[4];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void stopSoundLoop()
    {
        audioSource.Stop();
    }

    public void PlayRespawnDefeated()
    {
        audioSource.PlayOneShot(allSounds[6]);
    }
    
    
}
