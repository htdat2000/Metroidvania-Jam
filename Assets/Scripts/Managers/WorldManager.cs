using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public GameObject player;
    void Start() 
    {
        Instance = this;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }
}
