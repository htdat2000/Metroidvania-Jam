using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private AsyncOperation asyncLoad;
    
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
        CustomEvents.OnLoadingScreenActive();
        SceneManager.LoadSceneAsync("PlayerScene");
        //asyncLoad.allowSceneActivation = false;
        asyncLoad = SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Additive);
        yield return null;      
    }
}
