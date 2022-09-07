using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public static SFXManager sfxManager;
    AudioSource audioSource;
    [SerializeField] Slider sfxVolume;
    private AudioManager audioManager;
    
    void Awake() 
    {
        if(sfxManager != null)
        {
            Debug.LogError("More Than 1 SFXManager In Scene");
            return;
        }
        sfxManager = this;
        audioSource = GetComponent<AudioSource>();
    }
    void Start() 
    {
        audioManager = GetComponentInParent<AudioManager>();    
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if(this.gameObject.activeSelf)
        {
            audioSource.PlayOneShot(clip, audioManager.SFXVolume);
        }
    }

    public void AdjustVolume()
    {
        if(audioSource.volume > 0 && sfxVolume.value <= 0)
        {
            audioManager.SFXSwitch();
        }
        if(audioSource.volume <= 0 && sfxVolume.value > 0)
        {
            audioManager.SFXSwitch();
        }
        audioSource.volume = sfxVolume.value;
    }
}