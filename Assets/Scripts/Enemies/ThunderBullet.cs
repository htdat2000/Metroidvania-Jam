using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBullet : EnemyBullet
{
    protected override void Start()
    {

    }

    protected void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0)
            Destroy(this.gameObject);
        Move();
    }

    protected void Move()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    public void SetDir(Vector3 targetPos)
    {
        // transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan((targetPos.x - transform.position.x)/(targetPos.y - transform.position.y)));
        dir = targetPos.normalized;
    }

    // protected override void OnTriggerEnter2D(Collider2D col)
    // {
    //     base.OnTriggerEnter2D(col);
    //     Destroy(this.gameObject);
    // }
}
