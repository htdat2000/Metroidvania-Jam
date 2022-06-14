using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : NormalEnemy
{
    protected GameObject target;
    [SerializeField] Transform rayCastOrigin;

    protected override void Update()
    {
        AttackCountdown();
        if(target == null)
        {
            return;
        }
        ShootRayCast();
        CheckFlip();
    }

    protected void ShootRayCast()
    {
        Vector2 dir = (WorldManager.Instance.player.transform.position - this.gameObject.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin.position, dir, attackRange, LayerMask.GetMask("Ground"));
        if(hit.collider == null)
        {
            AttackAction();
        }
    }

    protected override void CheckFlip()
    {
        if((target.transform.position - this.gameObject.transform.position).normalized.x > 0)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
        base.CheckFlip();
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
