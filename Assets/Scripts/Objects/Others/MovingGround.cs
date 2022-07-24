using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    [SerializeField] protected Vector3 speed;
    protected int dir = 1;
    protected void FixUpdate()
    {
        this.gameObject.transform.Translate(speed * dir * Time.deltaTime);
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            dir = -dir;
        }
    }
}
