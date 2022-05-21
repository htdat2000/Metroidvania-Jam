using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float movement;
    private Rigidbody2D rb;
    private Animator anim;
    public bool isOnGround = false;
    public bool isNextToWall = false;

    private const float ANTI_SLIDE_ON_FLOOR = 0.05f;
    private const float MAX_FLOOR_SPEED = 20f;

    private bool isFacingRight = true;

    private enum State
    {
        Normal,
        Dashing
    }
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
        JumpCheck();
        RollCheck();
        AnimationUpdate();
        AutoFixXVelocity();
    }

    void HorizontalMove()
    {
        if(Input.GetKey("right"))
        {
            isFacingRight = true;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED),rb.velocity.y);
        }
        else if(Input.GetKey("left"))
        {
            isFacingRight = false;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed * -1, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED),rb.velocity.y);
        }
    }

    void AutoFlip()
    {
        transform.rotation = isFacingRight ? Quaternion.identity : Quaternion.Euler(0,180,0);
    }

    void JumpCheck()
    {
        if(Input.GetKeyDown("x"))// && isOnGround)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    void RollCheck()
    {
        if(Input.GetKeyDown("z"))// && isOnGround)
        {
            if(isFacingRight)
                rb.AddForce(new Vector2(jumpForce, 0), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(-jumpForce, 0), ForceMode2D.Impulse);
            // anim.Play("WDash");
        }
    }

    void AnimationUpdate()
    {
        if(Mathf.Abs(movement) > 0.01f && isOnGround)
        {
            anim.Play("Move");
        }
        if(Mathf.Abs(movement) <= 0.01f && isOnGround)
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
        // if(rb.velocity.x > 0.001f)
        //     rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - Time.deltaTime * ANTI_SLIDE_ON_FLOOR, 0f, rb.velocity.x),rb.velocity.y);
        // if(rb.velocity.x < 0.001f)
        //     rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * ANTI_SLIDE_ON_FLOOR, rb.velocity.x, 0f),rb.velocity.y);
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0f, ANTI_SLIDE_ON_FLOOR),rb.velocity.y);
    }
}
