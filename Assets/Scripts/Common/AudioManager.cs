using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
    public AudioClip[] audioClipArray;
    [HideInInspector]
    public AudioSource musicSource;
    [HideInInspector]
    public AudioSource soundfxSource;
    [HideInInspector]
    public AudioSource loopSource;

    void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = 1;
        musicSource.loop = false;

        soundfxSource = gameObject.AddComponent<AudioSource>();
        soundfxSource.loop = false;
        soundfxSource.volume = 0.7f;

        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.loop = true;
        loopSource.volume = 0.7f;

        instance = this;
        foreach (AudioClip ac in audioClipArray)
        {
            if (audioDict.ContainsKey(ac.name) == false)
            {
                audioDict.Add(ac.name, ac);
            }

        }
    }
    string lastName;
    // Use this for initialization
    public void Play(string audioName)
    {
        lastName = audioName;
        AudioClip ac;
        if (audioDict.TryGetValue(audioName, out ac))
        {
            musicSource.clip = ac;
            musicSource.Play();
        }
    }

    public void StopFX()
    {
        if (soundfxSource != null)
        {
            soundfxSource.Stop();
            soundfxSource.clip = null;
        }
    }
    public void Stop()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    public void PlayFX(string audioName)
    {
        AudioClip clip;
        if (audioDict.TryGetValue(audioName, out clip))
        {
            soundfxSource.clip = clip;
            soundfxSource.Play();
        }
    }
    public void RePlay()
    {
        Debug.Log(lastName);
        if (!musicSource.isPlaying && musicSource.clip != null)
            Play(lastName);
    }

    public void PlayLoop(string audioName)
    {
        AudioClip clip;
        if (audioDict.TryGetValue(audioName, out clip))
        {
            loopSource.clip = clip;
            loopSource.Play();
        }
    }

    public void StopLoop()
    {
        if (loopSource != null)
        {
            loopSource.clip = null;
            loopSource.Stop();
        }
    }
    public void StopAll()
    {
        if (loopSource != null)
        {
            loopSource.clip = null;
            loopSource.Stop();
        }
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
        if (soundfxSource != null)
        {
            soundfxSource.Stop();
            soundfxSource.clip = null;
        }
    }
}
