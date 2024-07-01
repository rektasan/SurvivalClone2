using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public AudioClip Clip => curClip;
    private AudioClip curClip;
    private AudioSource audioSource;
    private bool canPlayMultipleSounds;

    public void Setup(AudioClip clip, bool canPlayMultSounds)
    {
        audioSource = GetComponent<AudioSource>();
        curClip = clip;
        audioSource.clip = clip;
        canPlayMultipleSounds = canPlayMultSounds;
    }

    public void LoopSound(bool isNowLoop)
    {
        audioSource.loop = isNowLoop;
    }

    public void PlaySound()
    {
        if (audioSource.isPlaying && !canPlayMultipleSounds)
            return;

        audioSource.Play();
    }

    public void PauseSound()
    {
        if (!audioSource.isPlaying)
            return;

        audioSource.Pause();
    }
}