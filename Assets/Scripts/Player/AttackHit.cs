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
        Destroy(this.gameObject, 0.1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        IDamageable damageableObject;
        col.TryGetComponent<IDamageable>(out damageableObject);
        if(damageableObject != null)
        {
            damageableObject.TakeDmg(dmg);
        }
    }
}
