using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadGateData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void LoadGateData()
    {
        string gateData = PlayerPrefs.GetString("AllGates", "0000000000");
        // for(int i = 0; i < gateData.Length; i++)
        // {
        //     CustomEvents.OnLoadGateData?.Invoke(i, gateData[i] == '1');
        // }
    }
}
