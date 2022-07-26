using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] protected int defaultDmg;
    protected int dmg;

    protected virtual void Start()
    {
        dmg = defaultDmg;
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("[Traps] col is: " + col.gameObject.name);
        IDamageable target;
        col.gameObject.TryGetComponent<IDamageable>(out target);
        if(target == null)
        {
            Debug.Log("[Traps] col " + col.gameObject.name + " does not have IDamageable");
            return;
        }
        target.TakeDmg(dmg, null);
    }
}
