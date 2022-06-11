using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 30;
    [SerializeField] private float jumpForce = 7;
    [SerializeField] private float fastDashSpeed = 20;
    [SerializeField] private float slowDashSpeed = 12;
    [SerializeField] private float slideSpeed = 1;

    [Header("Counting")]
    private int comboCount = 0;
    private float comboResetTime = 1;
    private float comboCountdown = 1;

    private float jumpResetTime = 0.5f;
    private float jumpCountdown = 0;
    private int jumpCount = 0;
    private float movement;

    [Header("Component")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Bool Check Environment")]
    public bool isLastFrameOnGround = false;
    public bool isOnGround = false;
    public bool isNextToWall = false;
    private bool isFacingRight = true;

    [Header("Const")]
    private Vector3 DISTANCE_CENTER_TO_FEET = new Vector3(0f,-0.25f,0f);
    private const float ANTI_SLIDE_ON_FLOOR = 0.05f;
    private const float MAX_FLOOR_SPEED = 5f;
    private const float DASH_TIME = 0.15f;

    private enum State
    {
        Normal,
        Sliding,
        Rolling,
        Dashing,
        Attacking,
        Hooking,
        Dead
    }

    private State currentState = State.Normal;
    // Start is called before the first frame update

    [Header("Player Color Form")]
    private ColorForm currentColorForm = ColorForm.White;
    private enum ColorForm { White, Red, Blue, Yellow, Violet, Orange, Green};

    [Header("Other items")]
    [SerializeField] private GameObject singleAttackHit;
    [SerializeField] private GameObject[] comboAttackHits;
    [SerializeField] private GameObject aoeAttackHit;
    [SerializeField] private Hook hook;
    private TrailRenderer trail;
    // [Header("Debug")]
    
    void Awake()
    {
        CustomEvents.OnPlayerDied += PlayerDieBehaviour;
    }

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.widthMultiplier = 0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        Countdown();
        AutoFixXVelocity();
        if(currentState == State.Dead)
        {
            return;
        }
        HorizontalMove();
        GroundCheck();
        JumpCheck();
        ZButtonFunction();
        AttackCheck();
        SlideCheck();
        AnimationUpdate();
        SwitchForm();
        
    }

    void OnDestroy()
    {
        CustomEvents.OnPlayerDied -= PlayerDieBehaviour;
    }

    IEnumerator Attack(float waitime, string attackType)
    {
        yield return new WaitForSeconds(waitime);
        switch (attackType)
        {
            case "White":
                Instantiate(singleAttackHit, this.gameObject.transform.position, this.gameObject.transform.rotation);      
                break;
            case "Blue":
                Instantiate(aoeAttackHit, this.gameObject.transform.position, Quaternion.identity);
                break;
            case "Red":
                switch(comboCount)
                {
                    case 0:
                        comboCount++;
                        comboCountdown = comboResetTime;
                        Instantiate(comboAttackHits[0], this.gameObject.transform.position, this.gameObject.transform.rotation);
                        break;
                    case 1:
                        comboCount++;
                        comboCountdown = comboResetTime;
                        Instantiate(comboAttackHits[1], this.gameObject.transform.position, this.gameObject.transform.rotation);
                        break;
                    case 2:
                        comboCount++;
                        Instantiate(comboAttackHits[2], this.gameObject.transform.position, this.gameObject.transform.rotation);
                        break;
                    default:
                        comboCount = 0;
                        comboCountdown = comboResetTime;
                        break;
                }
                break;
            case "Yellow":
                switch(comboCount)
                {
                    case 0:
                        comboCount++;
                        comboCountdown = comboResetTime;
                        Instantiate(comboAttackHits[0], this.gameObject.transform.position, this.gameObject.transform.rotation);
                        break;
                    case 1:
                        comboCount++;
                        comboCountdown = comboResetTime;
                        Instantiate(comboAttackHits[1], this.gameObject.transform.position, this.gameObject.transform.rotation);
                        break;
                    default:
                        comboCount = 0;
                        comboCountdown = comboResetTime;
                        break;
                }
                break;
        }
    }

    void ComboCountdown()
    {   
        if(comboCount > 0)
        {
            if(comboCountdown > 0)
            {
                comboCountdown -= Time.deltaTime;
            }
            else
            {
                comboCount = 0;
            }
        }
    }

    void HorizontalMove()
    {
        if (currentState != State.Normal)
        {
            return;
        }
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
        isLastFrameOnGround = isOnGround;
        RaycastHit2D raycastHitR = Physics2D.Raycast(new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z), Vector2.down, 0.6f, platformLayerMask);
        RaycastHit2D raycastHitL = Physics2D.Raycast(new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z), Vector2.down, 0.6f, platformLayerMask);
        //Color rayColor;
        isOnGround = (raycastHitR.collider != null && raycastHitL.collider != null);
        if(isOnGround)
        {
            ResetJumpCount();
            if(!isLastFrameOnGround)
                EffectPool.Instance.GetLandingEffectInPool(transform.position + DISTANCE_CENTER_TO_FEET);
        }
    }

    void JumpCheck()
    {
        if (Input.GetKeyDown("x"))
        {
            switch (currentColorForm)
            {   
                case ColorForm.Red:
                    if(jumpCount < 1 && isOnGround)
                    {
                        Jump();
                    }
                    else if(!isOnGround && isNextToWall)
                    {
                        ResetJumpCount();
                        Jump();
                    }
                    break;
                case ColorForm.Yellow:
                    if(jumpCount < 1 && isOnGround)
                    {
                        Jump();
                    }
                    else if(!isOnGround && isNextToWall)
                    {
                        ResetJumpCount();
                        Jump();
                    }
                    break;
                case ColorForm.Blue:
                    if((jumpCount < 1) && isOnGround && (currentState != State.Hooking))
                    {
                        Jump();   
                    }
                    else if((jumpCount == 1) && (currentState != State.Hooking))
                    {
                        Jump();
                    }
                    break;
                default:
                    if(jumpCount < 1 && isOnGround)
                    {
                        Jump();
                    }
                    break;
            }
        }
    }

    void Countdown()
    {
        ComboCountdown();
        JumpCountdown();
    }

    void JumpCountdown()
    {
        if(jumpCountdown > 0)
        {
            jumpCountdown -= Time.deltaTime;
        }
    }

    void Jump()
    {
        if(jumpCountdown > 0)
        {
            return;
        }
        jumpCountdown = jumpResetTime;
        jumpCount++; 
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);    
        EffectPool.Instance.GetJumpEffectInPool(transform.position + DISTANCE_CENTER_TO_FEET);
    }

    void AttackCheck()
    {
        if (Input.GetKeyDown("c") && currentState == State.Normal)
        {
            switch (currentColorForm)
            {
                case ColorForm.White:
                    anim.Play("WAttack");
                    currentState = State.Attacking;
                    StartCoroutine(Attack(0.1f, "White"));
                    Invoke("BackToNormal", 0.5f);
                    break;
                case ColorForm.Blue:
                    anim.Play("BParry");
                    StartCoroutine(Attack(0.3f, "Blue"));
                    currentState = State.Attacking;
                    Invoke("BackToNormal", 0.5f);
                    break;
                case ColorForm.Red:
                    anim.Play("WAttack");
                    StartCoroutine(Attack(0.3f, "Red"));
                    currentState = State.Attacking;
                    Invoke("BackToNormal", 0.5f);
                    break;
                case ColorForm.Yellow:
                    anim.Play("WAttack");
                    StartCoroutine(Attack(0.3f, "Yellow"));
                    currentState = State.Attacking;
                    Invoke("BackToNormal", 0.5f);
                    break;
            }
        }

    }

    void ZButtonFunction()
    {   
        if (Input.GetKeyDown("z") && currentState == State.Normal)
        {
            switch (currentColorForm)
            {  
                case ColorForm.Red:
                    if(isOnGround == false) 
                    {
                        break;
                    }
                    DashCheck(fastDashSpeed);
                    break;
                case ColorForm.Yellow:
                    DashCheck(slowDashSpeed);
                    break;
                case ColorForm.Blue:
                    HookCheck();
                    break;
                default:
                    if(isOnGround)
                    {
                        RollCheck();
                    }
                    break;
            }
        }
    }

    void HookCheck()
    {
        if(hook.gameObject.activeSelf)
        {
            
            return;
        }
        else
        {
            anim.Play("BHook");
            currentState = State.Hooking;
            hook.gameObject.SetActive(true);
            hook.PushHook(isFacingRight);
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

    void DashCheck(float _speed)
    {
        if (isFacingRight)
            rb.velocity = new Vector2(_speed, 0);
        else
            rb.velocity = new Vector2(-_speed, 0);

        trail.widthMultiplier = 0.3f;
        anim.Play("RDash");
        currentState = State.Dashing;
        Invoke("BackToNormal", DASH_TIME);
    }
    void DisableTrail()
    {
        trail.widthMultiplier = 0f;
    }

    void SlideCheck()
    {

        if(currentColorForm != ColorForm.Red)
        {
            return;
        }
        

        if(isNextToWall && rb.velocity.y <0 && !isOnGround)

        {
            currentState = State.Sliding;
            anim.Play("Slide");
            rb.velocity = new Vector2(0, -slideSpeed);
        }
        else if(currentState == State.Sliding)
        {
            //currentState = State.Normal;
        }
    }

    public void BackToNormal()
    {
        currentState = State.Normal;
        DisableTrail();
    }

    void ResetJumpCount()
    {
        jumpCount = 0;
    }

    void SwitchForm()
    {
        currentState = State.Normal;
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
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0f, ANTI_SLIDE_ON_FLOOR), rb.velocity.y);
    }

    void PlayerDieBehaviour()
    {
        currentState = State.Dead;
        anim.Play("Dead");
        Invoke("BackToNormal", 2);
    }
}
