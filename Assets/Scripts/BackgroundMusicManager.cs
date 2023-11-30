using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip bgMusic;
    public AudioClip deathSFX;
    public AudioClip winSFX;

    bool bgMusicPlaying = true;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDeathSFX()
    {
        audioSource.clip = deathSFX;
        audioSource.loop = false;
        if (bgMusicPlaying)
        {
            audioSource.Play();
            bgMusicPlaying = false;
        }
    }
    public void PlayWinSFX()
    {
        audioSource.clip = winSFX;
        audioSource.loop = false;
        if (bgMusicPlaying)
        {
            audioSource.Play();
            bgMusicPlaying = false;
        }
    }
}
