using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
    private float currentMusicVolume;
    private float currentSFXVolume;
    [SerializeField] private GameObject audioSourcePrefab;
    private SoundMap soundMap;
    private AudioSource currentMusicSource;
    private List<AudioSource> sfxSources;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        sfxSources = new List<AudioSource>();

        currentSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);

        SoundEvents.SetSFXVolume += SetSFXVolume;
        SoundEvents.SetMusicVolume += SetMusicVolume;
    }
    private void OnDestroy()
    {
        SoundEvents.SetSFXVolume -= SetSFXVolume;
        SoundEvents.SetMusicVolume -= SetMusicVolume;
    }
    private void SetSFXVolume(float value)
    {
        currentSFXVolume = value;
    }
    private void SetMusicVolume(float value)
    {
        currentMusicVolume = value;
    }
    public void PlaySound(AudioClip audio, float volume = -1f)
    {
        AudioSource current;
        if (sfxSources.Count == 0)
        {
            current = CreateNewAudioSource();
        }
        else
        {
            current = GetFreeAudioSource();
        }

        if (current == null)
            return;
        current.clip = audio;
        if(volume < 0)
            current.volume = GetSFXVolume();
        current.Play();
    }
    public void PlaySound(string name, float volume = -1f)
    {
        if (soundMap == null)
            return;
        foreach (SoundAsset item in soundMap.items)
        {
            if(item.name == name)
            {
                PlaySound(item.audioClip, volume);
            }
        }
    }
    private float GetSFXVolume()
    {
        return currentSFXVolume;
    }
    private AudioSource CreateNewAudioSource()
    {
        AudioSource newSource = Instantiate(audioSourcePrefab).GetComponent<AudioSource>();
        DontDestroyOnLoad(newSource);
        sfxSources.Add(newSource);
        return newSource;
    }
    private AudioSource GetFreeAudioSource()
    {
        foreach(AudioSource source in sfxSources)
        {
            if(source.isPlaying == false)
            {
                return source;
            }
        }
        return CreateNewAudioSource();
    }
}
