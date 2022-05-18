using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float movement;
    private Rigidbody2D rb;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove();
        AutoFlip();
        JumpCheckk();
        AnimationUpdate();
        AutoFixXVelocity();
    }

    void HorizontalMove()
    {
        movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * moveSpeed;
    }

    void AutoFlip()
    {
        if(!Mathf.Approximately(0, movement))
            transform.rotation = movement > 0 ? Quaternion.identity : Quaternion.Euler(0,180,0);
    }

    void JumpCheckk()
    {
        if(Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
    }

    void AnimationUpdate()
    {
        if(Mathf.Abs(movement) > 0.01f && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            anim.Play("Move");
        }
        if(Mathf.Abs(movement) <= 0.01f && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            anim.Play("Idle");
        }
        if(rb.velocity.y < -0.001f)
        {
            anim.Play("AirDown");
        }
        if(rb.velocity.y > 0.001f)
        {
            anim.Play("AirUp");
        }
    }

    void AutoFixXVelocity()
    {
        rb.velocity = new Vector2(0f,rb.velocity.y);
    }
}
