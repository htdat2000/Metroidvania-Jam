using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : EnemyAttack
{
    protected Vector2 dir;
    [SerializeField] protected float speed = 3;

    protected override void Start()
    {
        SetDir();
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected void Move()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    public void SetDir()
    {
        dir = (WorldManager.Instance.player.transform.position - this.gameObject.transform.position).normalized;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        // Destroy(this.gameObject);
    }
}
