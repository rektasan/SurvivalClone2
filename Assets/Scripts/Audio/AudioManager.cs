using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip music;

    private List<AudioObject> audioObjects = new List<AudioObject>();

    private bool cantPlaySounds;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PlaySound(music, false);

        for (int i = 0; i < audioObjects.Count; i++)
        {
            if (audioObjects[i].Clip == music)
            {
                audioObjects[i].LoopSound(true);
                return;
            }
        }
    }

    public void PlaySound(AudioClip clip, bool canPlayMultSounds)
    {
        if (cantPlaySounds)
            return;

        for (int i = 0; i < audioObjects.Count; i++)
        {
            if (audioObjects[i].Clip == clip)
            {
                audioObjects[i].PlaySound();
                return;
            }
        }

        AudioObject audioObject = Instantiate(audioPrefab, transform)
            .GetComponent<AudioObject>();
        audioObject.Setup(clip, canPlayMultSounds);
        audioObject.PlaySound();
        audioObjects.Add(audioObject);
    }

    public void PauseSound(AudioClip clip)
    {
        for (int i = 0; i < audioObjects.Count; i++)
        {
            if (audioObjects[i].Clip == clip)
            {
                audioObjects[i].PauseSound();
                return;
            }
        }
    }

    public void BlockSounds(bool blocked)
    {
        cantPlaySounds = blocked;
        if(blocked)
        {
            PauseAllSounds();
        }
        else
        {
            PlaySound(music, false);
        }
    }

    public void PauseAllSounds()
    {
        for(int i = 0; i < audioObjects.Count; i++)
        {
            audioObjects[i].PauseSound();
        }
    }
}