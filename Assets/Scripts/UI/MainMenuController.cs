using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private AudioClip buttonSound;

    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider MusicSlider;
    private void Start()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
    }
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        buttonsPanel.SetActive(false);
    }
    public void OpenButtonsPanel()
    {
        settingPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }
    public void OnPlayPress()
    {
        SceneManager.LoadScene("DemoScene");
    }
    public void PlayButtonSound()
    {
        SoundController.Instance?.PlaySound(buttonSound);
    }
    public void OnSFXVolumeChange()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
        SoundEvents.SetSFXVolume?.Invoke(SFXSlider.value);
    }
    public void OnMusicVolumeChange()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
        SoundEvents.SetMusicVolume?.Invoke(MusicSlider.value);
    }
}
