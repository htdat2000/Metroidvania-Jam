using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField] Color[] colors = new Color[4];
    PlayerController playerController;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();     
        playerController = GetComponent<PlayerController>();  
    }

    void Update()
    {
        ChangeWispColor();
    }

    void ChangeWispColor()
    {
        switch (playerController.currentColorForm)
        {
            case PlayerController.ColorForm.White:
                spriteRenderer.color = colors[0];
                break;
            case PlayerController.ColorForm.Red:
                spriteRenderer.color = colors[1];
                break;
            case PlayerController.ColorForm.Blue:
                spriteRenderer.color = colors[2];
                break;
            case PlayerController.ColorForm.Yellow:
                spriteRenderer.color = colors[3];
                break;  
        }
    }
}
