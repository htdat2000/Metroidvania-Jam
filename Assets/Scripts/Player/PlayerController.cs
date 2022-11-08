using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : Movable
{
    public static PlayerController Instance;

    [SerializeField] float ATTACK_MAX_SPEED = 3f;
    [SerializeField] float COYOTE_TIME = 0.2f;
    [SerializeField] int MAX_EXTRA_JUMP = 1;
    [SerializeField] float DASH_INPUT_TIME = 0.25f;
    [SerializeField] float DASH_FORCE = 0.25f;
    [SerializeField] float DASH_TIME = 0.25f;
    [SerializeField] float TELE_COOLDOWN = 0.5f;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private int extraJump = 0;

    private float coyoteTimer = 0f;

    //private RaycastHit hit;

    private float moveLeft = 0f;
    private float moveRight = 0f;

    private bool isOnWall = false;

    private float lastMoveInputPress = 0f;
    private Tween dashDelay;

    [SerializeField] private PlayerTargetChecker targetChecker;
    private float teleTimer;

    [SerializeField] private PlayerAttackController attackController;

    [SerializeField] private PlayerChargeController chargeController;
    public bool IsCharging = false;

    private bool isHavingAtkFeedback = false;
    public CameraShake mainVCam;
    private void Awake() 
    {
        Instance = this;    
    }
    protected override void Start()
    {
        base.Start();
    }

    void Update () {
        switch (state)
        {
            case State.Normal:
                CoyoteTimeCount();
                DashTimeCount();
                TeleCooldownCounter();
                horizontalVector = GetInputVectorOnKey();
                ApplyHorizontalForce();
                ApplyFriction();
                DashCheck();
                JumpCheck();
                HookCheck();
                ApplyGravity();
                WallSlideCheck();
                SetFacing();
            break;
            case State.Dashing:
                BreakDash();
            break;
            case State.Sliding:
                WallSlideJumpCheck();
                WallSlideDropCheck();
                WallSlide();
            break;
            case State.Hurting:
                stunCounter -= Time.deltaTime;
                if (stunCounter <= 0)
                {
                    state = State.Normal;
                }
            break;
        }
        chargeController.ChargeCheck();
        attackController.AttackCheck();
        Move();
	}
    private void CoyoteTimeCount()
    {
        if(coyoteTimer >= 0)
            coyoteTimer -= Time.deltaTime;
    }
    private Vector2 GetInputVectorOnKey()
    {
        moveLeft = 0f;
        moveRight = 0f;
        if((Input.GetKey(KeyCode.LeftArrow) || CrossPlatformInputManager.GetButton("Left")) && !IsCharging)
            moveLeft = -1f;
        if((Input.GetKey(KeyCode.RightArrow) || CrossPlatformInputManager.GetButton("Right")) && !IsCharging)
            moveRight = 1f;
        return Vector2.right * (moveRight + moveLeft);
    }
    public void OnLeftPress()
    {

    }
    public Vector2 GetInputVector()
    {
        return horizontalVector;
    }
    private void DashTimeCount()
    {
        if(lastMoveInputPress >= 0)
            lastMoveInputPress -= Time.deltaTime;
    }
    private void DashCheck()
    {
        if(IsCharging)
            return;
        if((Input.GetKeyDown(KeyCode.RightArrow) || CrossPlatformInputManager.GetButtonDown("Right")) && isGrounded)
        {
            if(lastMoveInputPress > 0 && IsFacingRight)
            {
                Dash(true);
            }
            lastMoveInputPress = DASH_INPUT_TIME;
        }
        else if((Input.GetKeyDown(KeyCode.LeftArrow) || CrossPlatformInputManager.GetButtonDown("Left")) && isGrounded)
		{
            if(lastMoveInputPress > 0 && !IsFacingRight)
            {
                Dash(false);
            }
            lastMoveInputPress = DASH_INPUT_TIME;
        }
    }
    public bool IsDashing()
    {
        return (state == State.Dashing);
    }
    private void BreakDash()
    {
        if(!isGrounded)
        {
            dashDelay?.Kill();
            state = State.Normal;
            lastMoveInputPress = 0;
        }
    }
    private void Dash(bool isInputRight)
    {
        Debug.Log("Dash");
        if (IsFacingRight == isInputRight)
        {
            Debug.Log("Dash Inside");
            state = State.Dashing;
            int dashDir = IsFacingRight? 1: -1;
            motionVector.x = DASH_FORCE * dashDir;
            dashDelay = DOVirtual.DelayedCall(DASH_TIME, ()=> EndDashing());
        }
    }
    private void EndDashing()
    {
        state = State.Normal;
        lastMoveInputPress = 0;
    }

    private void SetFacing()
    {
        if(horizontalVector.x > 0 && !IsFacingRight && !IsAttacking)
        {
            Flip(true);
        }
        if(horizontalVector.x < 0 && IsFacingRight && !IsAttacking)
        {
            Flip(false);
        }
    }

    private void ApplyHorizontalForce()
    {
        if(horizontalVector != Vector2.zero)
        {
            motionVector.x += horizontalVector.x * acceleration * Time.deltaTime;
            if(!IsAttacking)
                motionVector.x = Mathf.Clamp(motionVector.x, -1 * maxSpeed, maxSpeed);
            else
                motionVector.x = Mathf.Clamp(motionVector.x, -1 * ATTACK_MAX_SPEED, ATTACK_MAX_SPEED);
        }
    }

    private void ApplyFriction()
    {
        if(horizontalVector == Vector2.zero)
        {
            if(isGrounded)
                motionVector.x = Mathf.Lerp(motionVector.x, 0, friction);
            else
                motionVector.x = Mathf.Lerp(motionVector.x, 0, friction /100f);
        }
    }
    private void JumpCheck()
    {
        if(isGrounded || coyoteTimer > 0f)
        {
            if(Input.GetKeyDown(KeyCode.X) || CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                motionVector.y = jumpForce;
            }
        }
        if(!isGrounded)
        {
            if((Input.GetKeyUp(KeyCode.X)|| CrossPlatformInputManager.GetButtonUp("Jump")) && motionVector.y > jumpForce/2f)
            {
                Debug.Log("Berak Jump");
                motionVector.y = jumpForce/2f;
            }
        }
        if(extraJump > 0 && !isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.X)|| CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                motionVector.y = jumpForce;
                extraJump --;
            }
        }
    }

    protected override void ApplyGravity()
    {
        base.ApplyGravity();
    }

    public override void SetGrounded(bool isGrounded)
    {
        if(this.isGrounded && !isGrounded)
        {
            coyoteTimer = COYOTE_TIME;
            attackController.SetLastInAir(false);
        }
        else if(!this.isGrounded && isGrounded)
        {
            extraJump = MAX_EXTRA_JUMP;
            motionVector.y = 0f;
            attackController.SetLastInAir(true);
        }
        this.isGrounded = isGrounded;
    }
    public bool GetGrounded()
    {
        return isGrounded;
    }
    public void SetOnWall(bool isOnWall)
    {
        if(!isOnWall)
        {
            state = State.Normal;
        }
        this.isOnWall = isOnWall;
    }
    private void WallSlideJumpCheck()
    {
        int isOnWallRight = IsFacingRight? 1: -1;
        if(Input.GetKeyDown(KeyCode.X) || CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            motionVector.x = isOnWallRight * maxSpeed * -0.5f;
            motionVector.y = jumpForce;
            state = State.Normal;
        }
    }
    private void WallSlideDropCheck()
    {
        if((Input.GetKeyDown(KeyCode.RightArrow) || CrossPlatformInputManager.GetButtonDown("Right")) && !IsFacingRight && !IsCharging)
        {
            motionVector.x = acceleration * Time.deltaTime;
            state = State.Normal;
        }
        if((Input.GetKeyDown(KeyCode.LeftArrow) || CrossPlatformInputManager.GetButtonDown("Left")) && IsFacingRight && !IsCharging)
        {
            motionVector.x = (-1 * acceleration) * Time.deltaTime;
            state = State.Normal;
        }   
    }
    private void WallSlide()
    {
        motionVector.y = Mathf.Max(motionVector.y - (gravity/2f) * Time.deltaTime, -1 * jumpForce/2f);
    }

    private void WallSlideCheck()
    {
        if(!isGrounded && isOnWall) 
        {
            Debug.Log("WTF");
            state= State.Sliding;
            extraJump = MAX_EXTRA_JUMP;
        }
    }

    private void HookCheck()
    {
        if(Input.GetKeyDown(KeyCode.Z) || CrossPlatformInputManager.GetButtonDown("Action"))
        {
            // state = State.Hooking;
            if(targetChecker.GetNearestTarget() != null && teleTimer <= 0)
            {
                transform.position = targetChecker.GetNearestTarget().position;
                motionVector.y = 0;
                teleTimer = TELE_COOLDOWN;
            }
        }
    }
    private void TeleCooldownCounter()
    {
        if(teleTimer >= 0)
        {
            teleTimer -= Time.deltaTime;
        }
    }
    public void ResetYMotion()
    {
        motionVector.y = 0;
    }
    public void SetYMotion(float value)
    {
        motionVector.y = value;
    }
    public void SetXMotion(float value)
    {
        motionVector.x = value;
    }
    public bool IsSliding()
    {
        return (state == State.Sliding);
    }
    public void HitEnemies()
    {
        if(isHavingAtkFeedback)
            return;
        isHavingAtkFeedback = true;
        StartCoroutine(DoneAtkFeedback());
        mainVCam.ShakeCamera(4, 0.1f);
        if(!isGrounded)
            motionVector.y = 2;
    }
    private IEnumerator DoneAtkFeedback()
    {
        yield return new WaitForSeconds(0.2f);
        isHavingAtkFeedback = false;
    } 
    public void DoHoldChargeBehavior()
    {
        if(IsSliding())
        {
            motionVector.y = 0;
        }
    }
    public override void GetHit(Vector2 forceVector, bool isRightOfPlayer, int dmg = 1)
    {
        if (state != State.Hurting)
        {
            base.GetHit(forceVector, isRightOfPlayer, dmg);
            mainVCam.ShakeCamera(3f, 0.1f);
        }
    }
}
