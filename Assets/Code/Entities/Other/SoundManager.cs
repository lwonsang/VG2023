using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource audioSource;
    public AudioClip[] allSounds;
    public float audioVolume = 0.2f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = audioVolume;
    }

    public void PlaySoundChar1LeftClick()
    {
        audioSource.PlayOneShot(allSounds[0]);
    }

    public void PlaySoundLevelUp()
    {
        audioSource.PlayOneShot(allSounds[1]);
    }

    public void PlaySoundGameOver()
    {
        audioSource.PlayOneShot(allSounds[3]);
    }

    public void PlaySoundChar1RightClick()
    {
        audioSource.clip = allSounds[2];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySoundChar2LeftClick()
    {
        audioSource.clip = allSounds[4];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySoundEnemy1Attack()
    {
        audioSource.PlayOneShot(allSounds[5]);
        
    }

    public void stopSoundLoop()
    {
        audioSource.Stop();
    }
    
    
}
