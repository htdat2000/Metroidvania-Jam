using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    public int gateID;
    public Sprite[] sprites;
    public GameEnum.GateStatus status = GameEnum.GateStatus.Locked;
    void Init()
    {
        CustomEvents.OnMapRefresh += SetCurrent;
        string data = PlayerPrefs.GetString("AllGates", "0000000000");
        SetLockSprite(data);
    }
    void OnDestroy() 
    {
        CustomEvents.OnMapRefresh -= SetCurrent;
    }
    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }
    void SetCurrent(int currentPort, string data)
    {
        if(gateID == currentPort)
        {
            gameObject.GetComponentInChildren<Image>().sprite = sprites[0];
        }
        else
        {
            SetLockSprite(data);
        }
    }
    void SetLockSprite(string data)
    {
        if(data[gateID] == '1')
            gameObject.GetComponentInChildren<Image>().sprite = sprites[1];
        else
            gameObject.GetComponentInChildren<Image>().sprite = sprites[2];
    }
}
