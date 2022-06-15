using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDog : NormalEnemy
{
    [SerializeField] private float randomMoveRate;
    [SerializeField] private float moveRateNoise;
    private float finalRandomMoveRate;
    private float lastMove;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        finalRandomMoveRate = FinalMoveRateCal();
        isMoveable = false;
        lastMove = Time.time;
    }

    // Update is called once per frame
    protected override void Update()
    {
        CheckMove();
        // AttackAction();
        base.Update();
    }
    protected override void Move()
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed * moveDir, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED), rb.velocity.y);
    }
    private bool CheckMove()
    {
        if(lastMove + finalRandomMoveRate <= Time.time)
        {
            lastMove = Time.time;
            finalRandomMoveRate = FinalMoveRateCal();
            isMoveable = !isMoveable;
            if(isMoveable)
            {
                anim.SetTrigger("Move");  
                enemyState = State.Walking;
            }
            else
            {
                anim.SetTrigger("Stop");
                enemyState = State.Normal;
            }
        }
        return isMoveable;
    }
    private float FinalMoveRateCal()
    {
        return finalRandomMoveRate + UnityEngine.Random.Range(-1f * moveRateNoise, moveRateNoise);
    }
    public override void AttackAction()
    {
        base.AttackAction();
    }
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player") && enemyState != State.Attacking)
        {
            IDamageable player = col.gameObject.GetComponent<IDamageable>();
            player.TakeDmg(dmg, this.gameObject);
            CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_DANGER_AMOUNT, GameConst.SHAKE_DANGER_TIME);
            EffectPool.Instance.GetHitEffectInPool(col.gameObject.transform.position);
        }
    }
}
