using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingPoint : MonoBehaviour
{   
    enum MapName{
        Null,
        DogArea1,
        DogArea2,
        DogArea3,
        DogArea4,
        DogArea5,
    }
    [SerializeField] MapName loadMap;
    [SerializeField] MapName unloadMap;

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if(loadMap.ToString() != "Null")
        {
            SceneManager.LoadSceneAsync(loadMap.ToString(), LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.UnloadSceneAsync(unloadMap.ToString());
        }
    }
}
