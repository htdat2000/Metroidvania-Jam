using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Wisp : MonoBehaviour
{
    // SpriteRenderer spriteRenderer;

    [SerializeField] Color[] colors = new Color[4];
    [SerializeField] Color[] lightColors = new Color[4];
    [SerializeField] PlayerController playerController;
    // public Color[] forms;
    [SerializeField] private SpriteRenderer soul;
    [SerializeField] private Light2D light2D;

    public void ChangeWispColor(int colorIndex)
    {
        switch (colorIndex)
        {
            case 0:
                soul.color = colors[0];
                light2D.color = lightColors[0];
                break;
            case 1:
                soul.color = colors[1];
                light2D.color = lightColors[1];
                break;
            case 2:
                soul.color = colors[2];
                light2D.color = lightColors[2];
                break;
            case 3:
                soul.color = colors[3];
                light2D.color = lightColors[3];
                break;  
        }
    }
}
