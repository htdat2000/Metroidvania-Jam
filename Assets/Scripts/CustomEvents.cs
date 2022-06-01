using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomEvents : MonoBehaviour
{
    static public Action<int> OnPlayerUnlock; //unlock index
    static public Action<int> OnTelepanelTrigger; //from gate index
}
