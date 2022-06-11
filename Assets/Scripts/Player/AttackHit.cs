using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    [SerializeField] private int dmg;
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
   
    void Start()
    {
        anim.Play(this.gameObject.name);
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Debug.Log("[AttackHit] col: " + col.gameObject.name);
        if(col.CompareTag("Player"))
        {
            return;
        }
        IDamageable damageableObject;
        col.TryGetComponent<IDamageable>(out damageableObject);
        if(damageableObject != null)
        {
            damageableObject.TakeDmg(dmg, null);
            CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_ATTACK_AMOUNT, GameConst.SHAKE_ATTACK_TIME);
            EffectPool.Instance.GetHitEffectInPool(col.gameObject.transform.position);
        }
    }

    public void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}
