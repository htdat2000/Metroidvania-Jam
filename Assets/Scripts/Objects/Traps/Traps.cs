using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] protected int dmg;
    
    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            IDamageable player = col.GetComponent<IDamageable>();
            player.TakeDmg(dmg, null);
        }
    }
}
