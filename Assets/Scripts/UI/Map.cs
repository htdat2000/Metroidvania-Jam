using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Map : MonoBehaviour
{
    [SerializeField] public TeleportData[] teleportData;

    void Init()
    {
    }
    void OnDestroy() 
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnEnable() { 
    }

    public void CloseMap()
    {
        gameObject.SetActive(false);
    }
    public void TeleTo(int _gateID)
    {
        string gateData = PlayerPrefs.GetString("AllGates", "0000000000");
        if(gateData[_gateID] == '1')
        {
            TeleportData targetTeleport = Array.Find(teleportData, teleport => teleport.gateID == _gateID);
            if(targetTeleport == null)
            {
                Debug.Log("No gate data to tele");
                return;
            }
            StartCoroutine(targetTeleport.Trigger());
            CloseMap();
        }
    }
}
