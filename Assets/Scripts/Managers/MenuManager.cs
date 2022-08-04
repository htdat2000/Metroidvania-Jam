using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        SceneManager.LoadSceneAsync("PlayerScene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
