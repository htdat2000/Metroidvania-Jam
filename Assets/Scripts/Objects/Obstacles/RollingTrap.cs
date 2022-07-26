using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingTrap : Traps
{
    [SerializeField] private int dir;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        Move();
    }

    void Update()
    {
        if(rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            rb.bodyType = RigidbodyType2D.Static;
            dmg = 0;
        }
        else
        {
            dmg = defaultDmg;
        }
    } 

    void Move()
    {
        rb.velocity = new Vector2(dir * speed, 0);
    }
}
