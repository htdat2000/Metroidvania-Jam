using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, IDamageable
{
    public static PlayerData Instance;
    public int defaultHP; // if change, should invoke OnMaxHPChange event
    private int hp;
    public static bool[] isColorActive = new bool[7] {true, true, true, false, false, false, false};
    //                                                white  red    blue   yel    vio    ora    gre
    enum State
    {
        Normal,
        Attacked
    }

    State playerState = State.Normal;
    Rigidbody2D rb;
    Animator anim;
    float lastHit;
    float hitCooldown = 0.5f;

    void Start() 
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        CustomEvents.OnPlayerUnlock += UnlockSkill;

        hp = defaultHP;
    }
    void OnDestroy() 
    {
        CustomEvents.OnPlayerUnlock -= UnlockSkill;
    }
    void Update()
    {
        if(lastHit > 0)
            lastHit -= Time.deltaTime;
    }
    void UnlockSkill(int index)
    {
        isColorActive[index] = true;
        Debug.Log("PlayerData: unlocked " + isColorActive);
    }

    public void TakeDmg(int _dmg, GameObject attacker)
    {
        if(lastHit <= 0)
        {
            lastHit = hitCooldown;
            if(playerState != State.Normal)
            {
                return;
            }
            playerState = State.Attacked;
            KnockbackEffect(attacker);
            DecreaseHp(_dmg);

            CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_ATTACK_AMOUNT, GameConst.SHAKE_ATTACK_TIME);
            EffectPool.Instance.GetHitEffectInPool(transform.position);
        }
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
        else CustomEvents.OnHPChange?.Invoke(hp);
    }

    void Die()
    {
        CustomEvents.OnPlayerDied?.Invoke();
        hp = defaultHP;
        CustomEvents.OnHPChange?.Invoke(hp);
    }

    public void KnockbackEffect(GameObject attacker)
    {
        if(attacker == null)
        {
            return;
        }

        Vector3 direction = this.gameObject.transform.position - attacker.transform.position;
        rb.AddForce(new Vector2(direction.normalized.x * 15, 0), ForceMode2D.Impulse);    
    }

    void ResetState()
    {
        playerState = State.Normal;
    }

    public void AddHp(int value)
    {
        hp += value;
        hp = Mathf.Clamp(hp, 0, defaultHP);
        CustomEvents.OnHPChange?.Invoke(hp);
    }
}
