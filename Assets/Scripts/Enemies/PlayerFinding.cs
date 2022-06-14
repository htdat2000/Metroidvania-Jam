using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinding : MonoBehaviour
{
    Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        GetComponent<CircleCollider2D>().radius = enemy.attackRange;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            enemy.AttackAction();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            enemy.FacePlayer();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            enemy.LostPlayer();
        }
    }
}
