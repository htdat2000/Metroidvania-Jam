using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEffect : MonoBehaviour
{
    public void DisableMe()
    {
        gameObject.SetActive(false);
    }
}
