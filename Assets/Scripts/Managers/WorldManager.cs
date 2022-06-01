using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public GameObject player;
    public GameObject map;
    void Init()
    {
        CustomEvents.OnTelepanelTrigger += OpenMap;
        Instance = this;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }
    void OnDestroy() 
    {
        CustomEvents.OnTelepanelTrigger -= OpenMap;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    void OpenMap(int currentPort)
    {
        map.SetActive(true);
    }
}
