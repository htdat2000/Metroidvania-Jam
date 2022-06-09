using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black : NormalEnemy
{
    [SerializeField] float attackRate = 1;
    float attackCountdown;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] float attackRange = 2;
    GameObject player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        anim.Play("Idle");
    }

    protected override void Update()
    {
        base.Update();
        AttackCountdown();
        if (DistanceBetweenPlayer())
        {
            Attack();
        }
    }
    bool DistanceBetweenPlayer()
    {
        if ((this.gameObject.transform.position - player.transform.position).magnitude <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Attack()
    {
        if (attackCountdown <= 0)
        {
            attackCountdown = attackRate;
            isMoveable = false;
            Instantiate(attackPrefab, this.gameObject.transform.position, Quaternion.identity);
            Invoke("ChangeMoveState", 1);
        }
    }

    protected void AttackCountdown()
    {
        if (attackCountdown > 0)
        {
            attackCountdown -= Time.deltaTime;
        }
    }
}
