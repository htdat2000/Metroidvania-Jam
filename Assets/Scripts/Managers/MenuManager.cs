using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvas;
    
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void Play()
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    public void Quit()
    {
        Application.Quit();
    }


    IEnumerator LoadYourAsyncScene()
    {
        loadingCanvas.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlayerScene");
        asyncLoad.allowSceneActivation = false;
        AsyncOperation asyncLoad_2 = SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            if(asyncLoad.progress == 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
                loadingCanvas.SetActive(false);
            }
            yield return null;
        }
    }


}
