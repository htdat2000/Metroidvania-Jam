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
    void Update() {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    void OnDestroy() 
    {
        CustomEvents.OnTelepanelTrigger -= OpenMap;
    }
    void Start()
    {
        Init();
    }
    void OpenMap(int currentPort)
    {
        map.SetActive(true);
        string gateData = PlayerPrefs.GetString("AllGates", "0000000000");
        CustomEvents.OnMapRefresh?.Invoke(currentPort, gateData);
    }
}
