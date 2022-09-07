using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public Music[] musics;
    AudioSource audioSource;
    [SerializeField] Slider musicVolume;
    private AudioManager audioManager;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }
    
    void Start() 
    {
        if(musics.Length != 0)
        {
            Play(musics[0].name);
        }
        audioManager = GetComponentInParent<AudioManager>();
    }

    public void Play(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if(m == null)
        {
            Debug.Log("Music: " + name + "Not Found");
            return;
        }
        audioSource.clip = m.audioClip;
        audioSource.volume = musicVolume.value;
        audioSource.Play();
    }

    public void Stop(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if(m == null)
        {
            Debug.Log("Music: " + name + "Not Found");
            return;
        }
        audioSource.Stop();
    }

    public void AdjustVolume()
    {
        if(audioSource.volume > 0 && musicVolume.value <= 0)
        {
            audioManager.MusicSwitch();
        }
        if(audioSource.volume <= 0 && musicVolume.value > 0)
        {
            audioManager.MusicSwitch();
        }
        audioSource.volume = musicVolume.value;
    }
}