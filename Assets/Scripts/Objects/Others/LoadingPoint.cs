using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingPoint : MonoBehaviour
{   
    enum MapName{
        Null,
        TestScene,
        DogArea1,
        DogArea2,
        DogArea3,
        DogArea4,
        DogArea5,
    }
    [SerializeField] MapName loadMap;
    [SerializeField] MapName unloadMap;
    [SerializeField] Vector3 posToGo;
    bool isLoaded = false;

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if(loadMap.ToString() != "Null" && (isLoaded == false) && col.CompareTag("Player"))
        {  
            isLoaded = true;
            col.gameObject.transform.position = posToGo;
            StartCoroutine("LoadTargetScene");
        }
    }

    IEnumerator LoadTargetScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadMap.ToString(), LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(unloadMap.ToString());
        yield return null;
    }
}
