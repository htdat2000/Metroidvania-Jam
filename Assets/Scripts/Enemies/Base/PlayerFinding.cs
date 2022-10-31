﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//It takes responsibility for triggering attack action method of enemy and check whether player enters attack range

[RequireComponent(typeof(Enemy))]
public class PlayerFinding : MonoBehaviour 
{
    bool hasTarget = false;
    Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        GetComponent<CircleCollider2D>().radius = enemy.attackRange;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player") && hasTarget == true)
        {
            enemy.AttackAction();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            hasTarget = true;
            enemy.FacePlayer();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            hasTarget = false;
            enemy.LostPlayer();
        }
    }
}
