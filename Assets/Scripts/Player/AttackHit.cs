using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
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
}
