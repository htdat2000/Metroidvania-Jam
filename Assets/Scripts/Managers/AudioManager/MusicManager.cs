using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
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
        SceneManager.sceneLoaded += PlayMusic;
    }
    
    void Start() 
    {
        audioManager = GetComponentInParent<AudioManager>();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= PlayMusic;
    }

    public void PlayMusic(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "PlayerScene")
        {
        GameObject target = GameObject.FindGameObjectWithTag("MusicCarrier");
        if(target != null)
        {
        Music targetMusic = target.GetComponent<MusicCarrier>().musicHolding;
        if(targetMusic != null)
        {
            audioSource.clip = targetMusic.audioClip;
            audioSource.volume = 1;//musicVolume.value;
            audioSource.Play();
        }
        }
        else
        {
            Debug.LogWarning("This Scene Don't Have Music Carrier");
        }
        }
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