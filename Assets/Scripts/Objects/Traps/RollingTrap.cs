using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingTrap : Traps
{
    [SerializeField] private int dir;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(dir * speed, 0);
    }
}
