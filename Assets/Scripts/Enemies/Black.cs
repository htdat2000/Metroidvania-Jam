using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black : NormalEnemy
{
    [SerializeField] float attackRange = 2;
    GameObject player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Update()
    {
        base.Update();
        if(DistanceBetweenPlayer())
        {
            Attack();
        }
    }
    bool DistanceBetweenPlayer()
    {
        if((this.gameObject.transform.position - player.transform.position).magnitude <= attackRange )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Attack()
    {
        Debug.Log("attack player");
    }
}
