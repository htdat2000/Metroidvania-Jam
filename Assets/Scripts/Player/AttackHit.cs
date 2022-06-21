using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    [SerializeField] private int dmg;
    
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
            damageableObject.TakeDmg(dmg, this.gameObject);
        }
    }

    public void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}
