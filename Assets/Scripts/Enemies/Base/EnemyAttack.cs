using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected int dmg;
    [SerializeField] protected float lifeTime = 5f;
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0)
            Destroy(this.gameObject);
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Cứu tao với Mực ơi");
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
