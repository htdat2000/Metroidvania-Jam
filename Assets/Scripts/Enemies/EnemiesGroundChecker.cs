using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGroundChecker : Checker
{
    private Enemies enemy;
    private void Start()
    {
        enemy = obj.GetComponent<Enemies>();
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Ground"))
            enemy.SetGrounded(true);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Ground"))
            enemy.SetGrounded(false);
    }
}
