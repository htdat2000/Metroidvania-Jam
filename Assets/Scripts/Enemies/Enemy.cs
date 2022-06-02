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
    
    protected virtual void Start()
    {
        hp = defaultHP;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void SetSpawnPoint(SpawnPoint newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;
        transform.position = newSpawnPoint.GetComponent<Transform>().position;
        gameObject.SetActive(true);
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        currentSpawnPoint = null;
    }

    public void TakeDmg(int _dmg, GameObject attacker)
    {
        GetHitBehaviour(_dmg);
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
}
