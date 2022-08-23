using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerMover : MonoBehaviour
//This class catch player input and call method from MoveController.cs.
{
    private Rigidbody2D rb;
    private float horizonMove;
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool isNextToWall;
    public Transform wallCheck;
    public float checkWallRadius;

    private MoveController moveController;

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
        Dashing
    }
    private PlayerState state = PlayerState.Normal;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        // moveController = new MoveController(gameObject, jumpForce);
        moveController = (new GameObject("MoveController")).AddComponent<MoveController>();
        moveController.InitParam(gameObject, jumpForce);
        currentForm = Form.Normal;
    }
    private void Update()
    {
        HorizontalMoveCheck();
        GroundCheck();
        InputCheck();
        SlideCheck();
    }
    private void FixedUpdate() 
    {
        Move();
    }
    private void Move()
    {
        if(state == PlayerState.Normal)
        {
            rb.velocity = new Vector2(horizonMove * speed, rb.velocity.y);
        }
    }
    private void HorizontalMoveCheck()
    {
        horizonMove = Input.GetAxisRaw("Horizontal");
    }
    private void GroundCheck()
    {
        UpdateIsGrounded();
        if(isGrounded)
            moveController.ExtraJumpRecover();
    }
    private void UpdateIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }
    private void UpdateIsNextToWall()
    {
        isNextToWall = Physics2D.OverlapCircle(wallCheck.position, checkWallRadius, whatIsGround);
    }
    private void InputCheck()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveController.Jump();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            moveController.Attack();
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveController.Charge();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(CheckLastHorizontalDirIs(HorizontalMoveDir.Left) && CanDash())
            {
                Dash((int)HorizontalMoveDir.Left);
            }
            lastHorizontalDir = HorizontalMoveDir.Left;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
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
        moveController.Dash(dir);
        SetPlayerState(PlayerState.Dashing);
    }
    public void BackToNormal()
    {
        Debug.Log("Alo");
        SetPlayerState(PlayerState.Normal);
    }
    private void SetPlayerState(PlayerState newState)
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
    private void SlideCheck()
    {

    }
}
