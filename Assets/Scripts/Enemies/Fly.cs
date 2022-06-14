using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : NormalEnemy
{
    protected GameObject target;
    protected Vector2 dirMove;

    public override void AttackAction()
    {
        return;
    }

    protected override void Update()
    {
        Move();
        TargetChecking();
        CheckFlip();
    }
    protected override void Move()
    {
        transform.Translate(dirMove * moveSpeed * Time.deltaTime, Space.World);
        //rb.velocity = dirMove * Time.deltaTime * moveSpeed;
    }

    protected void TargetChecking()
    {
        if(target == null)
        {
            if((this.gameObject.transform.position - currentSpawnPoint.gameObject.transform.position).magnitude > 0.2f)
            {
                ReturnSpawnPoint();
            }
            else
            {
                dirMove = Vector2.zero;
                return;
            }
        }
        else
        {
            if((this.gameObject.transform.position - currentSpawnPoint.gameObject.transform.position).magnitude > attackRange)
            {
                ReturnSpawnPoint();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    protected override void CheckFlip()
    {
        if(dirMove.x > 0)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
        base.CheckFlip();
    }

    protected void ReturnSpawnPoint()
    {
        dirMove = (currentSpawnPoint.gameObject.transform.position - this.gameObject.transform.position).normalized;
    }

    protected void ChasePlayer()
    {
        dirMove = (WorldManager.Instance.player.transform.position - this.gameObject.transform.position).normalized;
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
