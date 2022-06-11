using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] private Map map;
    private int gateID;
    private GameObject player;

    void Awake()
    {
        Init();
    }

    void OnDestroy()
    {
        CustomEvents.OnCheckpointSet -= SetCheckPoint;
        CustomEvents.OnPlayerDied -= RevivePlayer;
    }

    void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CustomEvents.OnCheckpointSet += SetCheckPoint;
        CustomEvents.OnPlayerDied += RevivePlayer;
    }

    void SetCheckPoint(int _gateID)
    {   
        gateID = _gateID;
    }

    void RevivePlayer()
    {   
        Invoke("MovePlayerPosition", 1);
    }

    void MovePlayerPosition()
    {
        player.transform.position = map.gate[gateID].transform.position;
    }
}
