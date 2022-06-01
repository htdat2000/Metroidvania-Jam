using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportButton : MonoBehaviour
{
    public int gateID;
    public Sprite[] sprites;
    void Init()
    {
        CustomEvents.OnTelepanelTrigger += SetCurrent;
    }
    void OnDestroy() 
    {
        CustomEvents.OnTelepanelTrigger -= SetCurrent;
    }
    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }
    void SetCurrent(int currentPort)
    {
        if(gateID == currentPort)
        {
            // Debug.Log("")
            gameObject.GetComponentInChildren<Image>().sprite = sprites[0];
        }
        else
        {
            gameObject.GetComponentInChildren<Image>().sprite = sprites[1];
        }
    }
}
