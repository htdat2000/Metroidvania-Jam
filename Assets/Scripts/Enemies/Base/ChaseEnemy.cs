using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : Enemy //this type of enemies will chase player when player within their active range.
{
    [SerializeField] protected float moveSpeed;
    protected bool isFacingRight = false;
    protected GameObject target;
    protected Vector2 dirMove;

    protected override void Update()
    {
        if(enemyState != State.Normal)
        {
            return;
        }
        Move();
        TargetChecking();
        CheckFlip();
    }
    protected virtual void Move()
    {
        transform.Translate(dirMove * moveSpeed * Time.deltaTime, Space.World);
        //rb.velocity = dirMove * Time.deltaTime * moveSpeed;
    }

    protected void TargetChecking()
    {
        if(target == null)
        {
            ReturnSpawnPoint();
        }
        else
        {
            if((target.gameObject.transform.position - currentSpawnPoint.gameObject.transform.position).magnitude > attackRange)
            {
                ReturnSpawnPoint();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    protected virtual void CheckFlip()
    {
        if(dirMove.x > 0)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
        transform.rotation = isFacingRight ?Quaternion.Euler(0, 180, 0) : Quaternion.identity;
    }

    protected void ReturnSpawnPoint()
    {
        if((this.gameObject.transform.position - currentSpawnPoint.gameObject.transform.position).magnitude > 0.3f)
        {
            dirMove = (currentSpawnPoint.gameObject.transform.position - this.gameObject.transform.position).normalized;
        }
        else
        {
            dirMove = Vector2.zero;
            return;
        }
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

    public override void AttackAction()
    {
        return;
    }
}
