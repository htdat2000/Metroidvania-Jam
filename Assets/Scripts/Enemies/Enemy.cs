using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected int defaultHP;
    protected int hp;
    [SerializeField] protected int dmg;
    protected bool isMoveable = true; 
    protected SpawnPoint currentSpawnPoint = null;
    protected Animator anim;

    [SerializeField] float attackRate = 1;
    float attackCountdown;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] GameObject attackSpawnPos;
    [SerializeField] float attackRange = 2;
    GameObject player;

    protected enum State
    {
        Normal,
        Attacking
    }
    protected State enemyState = State.Normal;
    
    
    protected virtual void Start()
    {
        hp = defaultHP;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        AttackCountdown();
        if (DistanceBetweenPlayer())
        {
            isMoveable = false;
            AttackAction();
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

    public void AttackAction()
    {
        if ((attackCountdown <= 0) && (enemyState == State.Normal))
        {
            enemyState = State.Attacking;
            anim.SetTrigger("Attack");
            attackCountdown = attackRate;   
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

    public void TakeDmg(int _dmg, GameObject attacker)
    {
        GetHitBehaviour(_dmg);
        CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_ATTACK_AMOUNT, GameConst.SHAKE_ATTACK_TIME);
        EffectPool.Instance.GetHitEffectInPool(transform.position);
        // Debug.Log("[Enemy] take dmg");
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

    public void CreateAttackPrefab()
    {
        enemyState = State.Normal;
        if(attackPrefab)
        {
            Instantiate(attackPrefab, attackSpawnPos.transform.position, attackSpawnPos.transform.rotation);
        }
    }
    public void SetIsMoveable()
    {
        isMoveable = true;
    }
}
