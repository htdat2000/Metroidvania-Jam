using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesHurtBox : Checker
{
    private Enemies enemy;
    private Vector2 forceVector;
    private bool isRightOfPlayer;
    private void Start()
    {
        enemy = obj.GetComponent<Enemies>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerAttack"))
        {
            forceVector = other.GetComponent<PlayerAttack>().ForceVector;
            isRightOfPlayer = (transform.position.x > PlayerController.Instance.transform.position.x);
            PlayerController.Instance.HitEnemies();
            enemy.GetHit(forceVector, isRightOfPlayer);
        }
    }
}
