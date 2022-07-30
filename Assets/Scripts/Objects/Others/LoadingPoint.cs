using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingPoint : MonoBehaviour
{   
    enum MapName{
        Null,
        BossDogArea1,
        BossDogArea2,
        BossDogArea3,
        BossDogArea4,
        BossDogArea5,
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
