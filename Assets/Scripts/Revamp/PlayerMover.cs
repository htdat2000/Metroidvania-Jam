using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerMover : MonoBehaviour
//This class catch player input and call method from MoveController.cs.
{
    [SerializeField] private float MAX_SPEED = 50f;
    [SerializeField] private float ACCELERATION = 0.05f;
    private Rigidbody2D rb;
    private float horizonMove;
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    private bool isGrounded;
    public bool IsGrounded
    {
        get {return isGrounded;}
        set {
            isGrounded = value;
            if(state == PlayerState.Sliding && isGrounded)
            {
                SetPlayerState(PlayerState.Normal);
            }
        }
    }
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool lastFrameNextToWall;
    private bool isNextToWall;
    private bool IsNextToWall
    {
        get {return isNextToWall;}
        set {
            lastFrameNextToWall = isNextToWall;
            isNextToWall = value;
            if(!isNextToWall)
            {
                NotNextToWall();
            };
        }
    }
    public Transform wallCheck;
    public float checkWallRadius;

    private bool isFacingRight = true;
    
    public bool IsFacingRight
    {
        get {return isFacingRight;}
        set {
            if(isFacingRight != value)
                Flip();
            isFacingRight = value;
        }
    }
    [SerializeField] private Transform model;

    // private MoveController moveController;

    private enum HorizontalMoveDir
    {
        Left = -1,
        Stand = 0,
        Right = 1
    }
    private HorizontalMoveDir lastHorizontalDir;
    private float lastHorizontalInputTime = 0f;
    [SerializeField] private float timeIntervalDashInput = 0.5f;

    public enum Form
    {
        Normal,
        Red,
        Blue,
        Yellow
    }
    [SerializeField] private Form currentForm;

    public enum PlayerState
    {
        Normal,
        Dashing,
        Sliding
    }
    public PlayerState state = PlayerState.Normal;
    private MoveSet moveControl;
    [SerializeField] private MoveSet[] Forms;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        InitForms();
        rb = GetComponent<Rigidbody2D>();
        moveControl = GetComponent<MoveSet>();
        moveControl = Forms[1];
        currentForm = Form.Normal;
    }
    private void InitForms()
    {
        foreach (MoveSet Form in Forms)
        {
            Form.InitParam(this.gameObject);
        }
    }
    private void Update()
    {
        HorizontalMoveCheck();
        GroundCheck();
        NextToWallCheck();
        InputCheck();
        Move();
        AddAcceleration();
    }
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallCheck.position, checkWallRadius);
    }
    private void FixedUpdate() 
    {
    }
    private void Move() //Will move to MoveSet
    {
        if(state == PlayerState.Normal && IsReadHorizonInput())
        {
            rb.AddForce(new Vector2(horizonMove * speed * Time.deltaTime, 0f)); // can optimize
        }
    }
    private void AddAcceleration()
    {
        float newX = Mathf.Lerp(rb.velocity.x, 0f, ACCELERATION);
        rb.velocity = new Vector2(newX, rb.velocity.y);
    }
    private bool IsReadHorizonInput()
    {
        if(rb.velocity.x * horizonMove < 0 || Mathf.Abs(rb.velocity.x) < MAX_SPEED)
        {
            return true;
        }
        return false;
    }
    private void HorizontalMoveCheck()
    {
        horizonMove = Input.GetAxisRaw("Horizontal");
    }
    private void GroundCheck()
    {
        UpdateIsGrounded();
        if(IsGrounded)
            moveControl.ExtraJumpRecover();
    }
    private void UpdateIsGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }
    private void UpdateIsNextToWall()
    {
        IsNextToWall = Physics2D.OverlapCircle(wallCheck.position, checkWallRadius, whatIsGround);
    }
    private void InputCheck()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveControl.Jump();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            moveControl.Attack();
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveControl.Charge();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(lastHorizontalDir != HorizontalMoveDir.Left)
            {
                IsFacingRight = false;
            }
            if(CheckLastHorizontalDirIs(HorizontalMoveDir.Left) && CanDash())
            {
                Dash((int)HorizontalMoveDir.Left);
            }
            lastHorizontalDir = HorizontalMoveDir.Left;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(lastHorizontalDir != HorizontalMoveDir.Right)
            {
                IsFacingRight = true;
            }
            if(CheckLastHorizontalDirIs(HorizontalMoveDir.Right) && CanDash())
            {
                Dash((int)HorizontalMoveDir.Right);
            }
            lastHorizontalDir = HorizontalMoveDir.Right;
        }

        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            UpdateLastHorizontalInput();
        }
    }
    private void Dash(int dir)
    {
        moveControl.Dash(dir);
        SetPlayerState(PlayerState.Dashing);
    }
    public void BackToNormal()
    {
        SetPlayerState(PlayerState.Normal);
    }
    public void SetPlayerState(PlayerState newState)
    {
        state = newState;
    }
    private void UpdateLastHorizontalInput()
    {
        lastHorizontalInputTime = Time.time;
    }
    private bool CheckLastHorizontalDirIs(HorizontalMoveDir checkVal)
    {
        return lastHorizontalDir == checkVal;
    }
    private bool CanDash()
    {
        bool passTimeCondition = (Time.time - lastHorizontalInputTime) <= timeIntervalDashInput;
        bool passStateCondition = (state == PlayerState.Normal);

        return passTimeCondition && passStateCondition;
    }
    private void Flip()
    {
        float currentXScale = model.localScale.x;
        float flipXScale = currentXScale * (-1);

        Vector3 result = new Vector3(flipXScale, model.localScale.y, model.localScale.z);

        model.localScale = result;
    }
    private void NextToWallCheck()
    {
        UpdateIsNextToWall();

        if(IsNextToWall && !IsGrounded && rb.velocity.y <= 0)
            ChangeToSlideState();
    }
    private void ChangeToSlideState()
    {
        moveControl.Slide();
    }
    private void NotNextToWall()
    {
        moveControl.QuitSlide();
    }
}
