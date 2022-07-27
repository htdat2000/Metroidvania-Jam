using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected int defaultHP;
    protected int hp;
    [SerializeField] protected int dmg;
    public bool isMoveable = true; 
    protected SpawnPoint currentSpawnPoint = null;
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

    protected const float HURT_TIME = 1f;
    
    
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

    public virtual void SetSpawnPoint(SpawnPoint newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;
        transform.position = newSpawnPoint.GetComponent<Transform>().position;
        gameObject.SetActive(true);
        isMoveable = true;
    }

    public void Despawn()
    {
        hp = defaultHP;
        gameObject.SetActive(false);
        currentSpawnPoint.BackEnemyToPool();
        currentSpawnPoint = null;
    }

    public virtual void TakeDmg(int _dmg, GameObject attacker)
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
    
    protected virtual void GetHitBehaviour(int _dmg)
    {
        DecreaseHP(_dmg);
    }

    protected virtual void DecreaseHP(int _dmg)
    {
        hp -= _dmg;
        hp = Mathf.Clamp(hp, 0, defaultHP);
        if(hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Despawn();
    }

    public virtual void CreateAttackPrefab()
    {
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

    protected virtual void BackToNormal()
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
