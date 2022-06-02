﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, IDamageable
{
    [SerializeField] private int defaultHP;
    private int hp;
    public static bool[] isColorActive = new bool[7] {true, false, false, false, false, false, false};
    //                                                white  red    blue   yel    vio    ora    gre
    enum State
    {
        Normal,
        Attacked
    }

    State playerState = State.Normal;
    Rigidbody2D rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        CustomEvents.OnPlayerUnlock += UnlockSkill;

        hp = defaultHP;
    }
    void OnDestroy() 
    {
        CustomEvents.OnPlayerUnlock -= UnlockSkill;
    }
    void UnlockSkill(int index)
    {
        isColorActive[index] = true;
        Debug.Log("PlayerData: unlocked " + isColorActive);
    }

    public void TakeDmg(int _dmg, GameObject attacker)
    {
        if(playerState != State.Normal)
        {
            return;
        }
        playerState = State.Attacked;
        KnockbackEffect(attacker);
        DecreaseHp(_dmg);
    }

    void DecreaseHp(int _dmg)
    {
        Invoke("ResetState", 1f);
        hp -= _dmg;
        hp = Mathf.Clamp(hp, 0, defaultHP);
        if(hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
    }

    public void KnockbackEffect(GameObject attacker)
    {
        Vector3 direction = this.gameObject.transform.position - attacker.transform.position;
        rb.AddForce(new Vector2(direction.normalized.x * 15, 0), ForceMode2D.Impulse);    
    }

    void ResetState()
    {
        playerState = State.Normal;
    }
}
