using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float slideSpeed;
    private int jumpCount = 0;
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
    private const int DASH_TIME = 1;


    private bool isFacingRight = true;

    private enum State
    {
        Normal,
        Rolling,
        Dashing
    }

    private State currentState = State.Normal;
    // Start is called before the first frame update

    [Header("Player Color Form")]
    private ColorForm currentColorForm = ColorForm.White;
    private enum ColorForm { White, Red, Blue, Yellow, Violet, Orange, Green};

    // [Header("Debug")]

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove();
        GroundCheck();
        JumpCheck();
        RollAndDash();
        SlideCheck();
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
        AutoFlip();
    }

    void AutoFlip()
    {
        transform.rotation = isFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    void GroundCheck()
    {
        RaycastHit2D raycastHitR = Physics2D.Raycast(new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z), Vector2.down, 0.6f, platformLayerMask);
        RaycastHit2D raycastHitL = Physics2D.Raycast(new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z), Vector2.down, 0.6f, platformLayerMask);
        Color rayColor;
        isOnGround = (raycastHitR.collider != null && raycastHitL.collider != null);
        if(isOnGround)
        {
            ResetJumpCount();
        }
    }

    void JumpCheck()
    {
        if (Input.GetKeyDown("x"))
        {
            switch (currentColorForm)
            {    
                case ColorForm.Blue:
                    if(jumpCount < 1 && isOnGround)
                    {
                        jumpCount++;
                        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);    
                    }
                    else if(jumpCount == 1)
                    {
                        jumpCount++;
                        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);    
                    }
                    break;
                default:
                    if(jumpCount < 1 && isOnGround)
                    {
                        jumpCount++;
                        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    }
                    break;
            }
        }
    }

    void RollAndDash()
    {
        if (Input.GetKeyDown("z") && currentState == State.Normal)
        {
            switch (currentColorForm)
            {
                case ColorForm.White:
                    if(isOnGround)
                    {
                        RollCheck();
                    }
                    break;
                case ColorForm.Red:
                    DashCheck();
                    break;
            }
        }
    }

    void RollCheck()
    {
        //if (Input.GetKeyDown("z") && currentState == State.Normal && isOnGround)
        //{
        if (isFacingRight)
            rb.AddForce(new Vector2(jumpForce, 0), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(-jumpForce, 0), ForceMode2D.Impulse);
        anim.Play("WDash");
        currentState = State.Rolling;
        Invoke("BackToNormal", 0.5f);
        //}
    }

    void DashCheck()
    {
        if (isFacingRight)
            rb.velocity = new Vector2(dashSpeed, 0);
        else
            rb.velocity = new Vector2(-dashSpeed, 0);
        //anim.Play("WDash");
        currentState = State.Dashing;
        Invoke("BackToNormal", DASH_TIME);
    }

    void SlideCheck()
    {
        if(isNextToWall && rb.velocity.y <0)
        {
            rb.velocity = new Vector2(0, -slideSpeed);
        }
    }

    void BackToNormal()
    {
        currentState = State.Normal;
        rb.velocity = Vector2.zero;
    }

    void ResetJumpCount()
    {
        jumpCount = 0;
    }

    void SwitchForm()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int numOfColor = PlayerData.isColorActive.Length;
            if((int)currentColorForm == (numOfColor - 1))   //last color
                {
                    currentColorForm = ColorForm.White;
                    return;
                }
            for (int i = 0; i < numOfColor; i++)
            {
                if (i <= (int)currentColorForm) //skip previous and current color
                {
                    continue;
                }
                
                if (PlayerData.isColorActive[i])
                {
                    currentColorForm = (ColorForm)i;
                    return;
                }
                else
                {
                    currentColorForm = ColorForm.White;
                }
            }
        }
    }

    void AnimationUpdate()
    {
        if (currentState == State.Normal)
        {
            if (Mathf.Abs(movement) > 0.01f && isOnGround)
            {
                anim.Play("Move");
            }
            if (Mathf.Abs(movement) <= 0.01f && isOnGround)
            {
                anim.Play("Idle");
            }
            if (rb.velocity.y < -0.001f && !isOnGround)
            {
                anim.Play("AirDown");
            }
            if (rb.velocity.y > 0.001f && !isOnGround)
            {
                anim.Play("AirUp");
            }
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
