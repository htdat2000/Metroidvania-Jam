using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvas;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CustomEvents.OnLoadingScreenActive += ActiveLoadingScreen;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CustomEvents.OnLoadingScreenActive -= ActiveLoadingScreen;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "PlayerScene")
        {
            loadingCanvas.SetActive(false);
        }
    }

    void ActiveLoadingScreen()
    {
        loadingCanvas.SetActive(true);
    }
}
