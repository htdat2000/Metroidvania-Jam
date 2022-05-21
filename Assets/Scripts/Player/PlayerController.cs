using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float movement;

    [Header("Component")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Bool Check Environment")]
    public bool isOnGround = false;
    public bool isNextToWall = false;

    [Header("Const")]
    private const float ANTI_SLIDE_ON_FLOOR = 0.05f;
    private const float MAX_FLOOR_SPEED = 20f;

    private bool isFacingRight = true;
    // Start is called before the first frame update

    [Header("Player Color Form")]
    private bool[] isColorActive = { true, true, false };
    private ColorForm currentColorForm = ColorForm.White;
    private enum ColorForm { White, Red, Blue };

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
        SwitchForm();
    }

    void HorizontalMove()
    {
        if (Input.GetKey("right"))
        {
            isFacingRight = true;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED), rb.velocity.y);
        }
        else if (Input.GetKey("left"))
        {
            isFacingRight = false;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed * -1, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED), rb.velocity.y);
        }
    }

    void AutoFlip()
    {
        transform.rotation = isFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    void JumpCheck()
    {
        if (Input.GetKeyDown("x"))// && isOnGround)
        {
            switch (currentColorForm)
            {
                case ColorForm.White:
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    break;
                case ColorForm.Red:
                    break;
                default:
                    break;
            }
        }
    }

    void RollCheck()
    {
        if (Input.GetKeyDown("z"))// && isOnGround)
        {
            rb.AddForce(new Vector2(jumpForce, 0), ForceMode2D.Impulse);
        }
    }

    void SwitchForm()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int numOfColor = isColorActive.Length;
            for (int i = 0; i < numOfColor - 1; i++)
            {
                if (i == (int)currentColorForm)
                {
                    continue;
                }
                if (isColorActive[i])
                {
                    currentColorForm = (ColorForm)i;
                    break;
                }
            }
        }
    }

    void AnimationUpdate()
    {
        if (Mathf.Abs(movement) > 0.01f && isOnGround)
        {
            anim.Play("Move");
        }
        if (Mathf.Abs(movement) <= 0.01f && isOnGround)
        {
            anim.Play("Idle");
        }
        if (rb.velocity.y < -0.001f)
        {
            anim.Play("AirDown");
        }
        if (rb.velocity.y > 0.001f)
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
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0f, ANTI_SLIDE_ON_FLOOR), rb.velocity.y);
    }
}
