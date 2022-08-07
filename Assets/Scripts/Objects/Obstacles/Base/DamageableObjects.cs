﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObjects : SpawnableObjects, IDamageable
{
    [SerializeField] protected int defaultHP;
    protected int hp;
    protected const float HURT_TIME = 0.5f;

    private enum State
    {
        Normal,
        Hurting
    }

    private State objectState = State.Normal;

    public override void Despawn()
    {
        base.Despawn();
        hp = defaultHP;
    }

    public virtual void TakeDmg(int _dmg, GameObject attacker)
    {
        if(objectState != State.Hurting)
        {
            Invoke("BackToNormal", HURT_TIME);
            objectState = State.Hurting;
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

    protected virtual void BackToNormal()
    {
        objectState = State.Normal;
    }
}