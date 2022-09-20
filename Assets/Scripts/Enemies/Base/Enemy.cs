using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamageableObjects
{ 
    [SerializeField] protected int dmg;
    public bool isMoveable = true; 
    protected Animator anim;
    protected Rigidbody2D rb;

    [SerializeField] protected float attackRate = 1;
    protected float attackCountdown;
    [SerializeField] protected GameObject attackPrefab;
    [SerializeField] protected GameObject attackSpawnPos;
    [SerializeField] public float attackRange = 2;

    protected enum State
    {
        Normal,
        Walking,
        Attacking,
        SpecialMove1,
        Hurting
    }
    protected State enemyState = State.Normal;

    protected virtual void Start()
    {
        hp = defaultHP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        AttackCountdown();
        if(enemyState != State.Normal)
        {
            rb.velocity = Vector2.zero;
            return;
        }
    }

    public virtual void AttackAction()
    {
        if ((attackCountdown <= 0) && (enemyState == State.Normal))
        {
            // attackCountdown = attackRate;
            enemyState = State.Attacking;
            anim.SetTrigger("Attack");
        }
    }

    protected void AttackCountdown()
    {
        if (attackCountdown > 0)
        {
            attackCountdown -= Time.deltaTime;
        }
    }

    public override void TakeDmg(int _dmg, GameObject attacker)
    {
        if(enemyState != State.Hurting)
        {
            Invoke("BackToNormal", HURT_TIME);
            enemyState = State.Hurting;
            GetHitBehaviour(_dmg);
            CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_ATTACK_AMOUNT, GameConst.SHAKE_ATTACK_TIME);
            EffectPool.Instance.GetHitEffectInPool(transform.position);
            //Debug.Log("[Enemy] take dmg");
        }
    }

    public virtual void CreateAttackPrefab()
    {
        PlaySFX(SFX.SFXState.AttackSFX);
        anim.ResetTrigger("Attack");
        enemyState = State.Normal;
        attackCountdown = attackRate;
        if(attackPrefab)
        {
            Instantiate(attackPrefab, attackSpawnPos.transform.position, attackSpawnPos.transform.rotation);
        }
    }

    public virtual void FacePlayer()
    {
        isMoveable = false;
    }

    public virtual void LostPlayer()
    {
        isMoveable = true;
    }

    protected override void BackToNormal()
    {
        enemyState = State.Normal;
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            IDamageable player = col.gameObject.GetComponent<IDamageable>();
            player.TakeDmg(dmg, this.gameObject);
            CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_DANGER_AMOUNT, GameConst.SHAKE_DANGER_TIME);
            EffectPool.Instance.GetHitEffectInPool(col.gameObject.transform.position);
        }
    }
}
