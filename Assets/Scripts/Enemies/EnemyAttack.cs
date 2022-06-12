using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int dmg;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemies"))
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

    public void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}
