﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegate : MonoBehaviour
{
    public int gateID;
    private bool isTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up") && isTrigger)
        {
            ShowTelePanel();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            isTrigger = false;
        }
    }

    void ShowTelePanel()
    {
        CustomEvents.OnTelepanelTrigger?.Invoke(gateID);
    }
}
