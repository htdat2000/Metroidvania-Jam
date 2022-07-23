using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrap : Traps
{
    protected bool isTrigger = false;
    protected Animation anim;

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player") && isTrigger)
        {
            anim.Play();
        }
    }
}
