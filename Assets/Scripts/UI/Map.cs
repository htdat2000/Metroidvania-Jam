using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] GameObject[] gate;
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
    public void TeleTo(int gateID)
    {
        string gateData = PlayerPrefs.GetString("AllGates", "0000000000");
        if(gateData[gateID] == '1')
        {
            WorldManager.Instance.player.transform.position = gate[gateID].transform.position;
            gameObject.SetActive(false);
        }
    }
}
