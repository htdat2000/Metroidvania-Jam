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
    [SerializeField] float attackRange = 2;
    GameObject player;
    
    
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
        else
        {
            isMoveable = true;
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
        if (attackCountdown <= 0)
        {
            // anim.SetBool("isAttack", true);
            anim.SetTrigger("Attack");
            attackCountdown = attackRate; 
            attackCountdown = attackRate;
            if(attackPrefab)
                Instantiate(attackPrefab, this.gameObject.transform.position, Quaternion.identity);   
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
        anim.SetBool("isAttack", false);
        if(attackPrefab)
        {
            Instantiate(attackPrefab, this.gameObject.transform.position, Quaternion.identity);
        }
    }
}
