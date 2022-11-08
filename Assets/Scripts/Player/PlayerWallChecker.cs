using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallChecker : Checker
{
    private PlayerController player;
    private void Start()
    {
        player = obj.GetComponent<PlayerController>();
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Ground"))
            player.SetOnWall(true);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Ground"))
            player.SetOnWall(false);
    }
}
