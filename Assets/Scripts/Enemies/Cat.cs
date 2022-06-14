using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : NormalEnemy
{
    protected GameObject target;
    [SerializeField] Transform rayCastOrigin;

    protected override void Update()
    {
        TargetChecking();
        CheckFlip();
    }

    protected void TargetChecking()
    {
        if(target == null)
        {
            return;
        }
        else
        {
            ShootRayCast();
        }
    }

    protected void ShootRayCast()
    {
        Vector2 dir = (WorldManager.Instance.player.transform.position - this.gameObject.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin.position, dir, attackRange);
        if(hit.collider.CompareTag("Player"))
        {
            AttackAction();
        }
    }

    public override void FacePlayer()
    {
        target =  WorldManager.Instance.player;
    }

    public override void LostPlayer()
    {
        target = null;
    }
}
