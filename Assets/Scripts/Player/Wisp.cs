using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Wisp : MonoBehaviour
{
    // SpriteRenderer spriteRenderer;

    [SerializeField] Color[] colors = new Color[4];
    [SerializeField] Color[] lightColors = new Color[4];
    PlayerController playerController;
    // public Color[] forms;
    [SerializeField] private SpriteRenderer soul;
    [SerializeField] private Light2D light2D;

    void Start()
    {     
        playerController = GetComponent<PlayerController>();  
    }

    public void ChangeWispColor()
    {
        switch (playerController.currentColorForm)
        {
            case PlayerController.ColorForm.White:
                soul.color = colors[0];
                light2D.color = lightColors[0];
                break;
            case PlayerController.ColorForm.Red:
                soul.color = colors[1];
                light2D.color = lightColors[1];
                break;
            case PlayerController.ColorForm.Blue:
                soul.color = colors[2];
                light2D.color = lightColors[2];
                break;
            case PlayerController.ColorForm.Yellow:
                soul.color = colors[3];
                light2D.color = lightColors[3];
                break;  
        }
    }
}
