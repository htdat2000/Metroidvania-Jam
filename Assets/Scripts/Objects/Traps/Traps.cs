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

    protected virtual void OnCollisionStay2D(Collision2D col)
    {
        IDamageable target;
        col.gameObject.TryGetComponent<IDamageable>(out target);
        if(target == null)
        {
            return;
        }
        target.TakeDmg(dmg, null);
    }
}
