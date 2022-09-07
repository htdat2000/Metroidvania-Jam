using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] Image musicImg;
    [SerializeField] Sprite musicIcon;
    [SerializeField] Sprite muteMusicIcon;
    [SerializeField] protected MusicManager musicManager;
    [SerializeField] AudioSource musicSource;
    private float lastMusicVolume;

    [Header("SFX")]
    [SerializeField] Image sfxImg;
    [SerializeField] Sprite sfxIcon;
    [SerializeField] Sprite muteSFXIcon;
    [SerializeField] protected SFXManager sfxManager;
    [SerializeField] AudioSource SFXSource;
    public float SFXVolume;
     
    public void MusicSwitch()
    {
        if(musicSource.volume > 0)
        {
            lastMusicVolume = musicSource.volume;
            SetMuteMusicIcon(true);
            musicSource.volume = 0;
        }
        else
        {
            SetMuteMusicIcon(false);
            musicSource.volume = lastMusicVolume;
        }
    }
    public void SetMuteMusicIcon(bool isMute)
    {
        if(isMute)
            musicImg.sprite = muteMusicIcon;
        else
            musicImg.sprite = musicIcon;
    }

    public void SFXSwitch()
    {
        if(SFXSource.volume > 0)
        {
            SFXVolume = SFXSource.volume;
            SetMuteSFXIcon(true);
            SFXSource.volume = 0;
        }
        else
        {
            SetMuteSFXIcon(false);
            SFXSource.volume = SFXVolume;
        }
    }
    public void SetMuteSFXIcon(bool isMute)
    {
        if(isMute)
            sfxImg.sprite = muteSFXIcon;
        else
            sfxImg.sprite = sfxIcon;
    }
}