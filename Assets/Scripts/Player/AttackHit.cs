using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    [SerializeField] private int dmg;
    [SerializeField] private float destroyTime;
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
   
    void Start()
    {
        anim.Play(this.gameObject.name);
        Destroy(this.gameObject, destroyTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            return;
        }
        IDamageable damageableObject;
        col.TryGetComponent<IDamageable>(out damageableObject);
        if(damageableObject != null)
        {
            damageableObject.TakeDmg(dmg, null);
        }
    }
}
