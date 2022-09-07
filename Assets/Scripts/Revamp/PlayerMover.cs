using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerMover : MonoBehaviour
//This class catch player input and call method from MoveController.cs.
{
    private Rigidbody2D rb;
    private float horizonMove;

    public bool IsGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool lastFrameNextToWall;
    private bool IsNextToWall;
    public Transform wallCheck;
    public float checkWallRadius;
    public bool IsFacingRight = true;
    
    [SerializeField] private float MAX_SPEED = 50f;
    [SerializeField] private float ACCELERATION = 0.05f;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform model;
    [SerializeField] private Form currentForm;
    [SerializeField] private MoveSet[] Forms;
    [SerializeField] Transform target; //Should move to another class

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
        Normal = 0,
        Red = 1,
        Blue = 2,
        Yellow = 3
    }
    public enum PlayerState
    {
        Normal,
        Attacking,
        Dashing,
        Sliding,
        Hooking
    }
    public PlayerState State = PlayerState.Normal;
    private MoveSet moveControl;
    
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        InitForms();
        rb = GetComponent<Rigidbody2D>();
        moveControl = GetComponent<MoveSet>();
        SetCombatForm(Form.Red); // for debug
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
        FacingCheck();

        // transform.Translate(Vector3.up * Time.deltaTime, Space.World);
    }
    private void FacingCheck()
    {
        SetIsFacingRight((int)lastHorizontalDir == 1);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallCheck.position, checkWallRadius);
    }
    private void FixedUpdate() 
    {
    }
    private void Move() //Will move to MoveSet
    {
        if(State == PlayerState.Normal && IsReadHorizonInput())
        {
            rb.AddForce(new Vector2(horizonMove * speed * Time.deltaTime, 0f)); // can optimize
        }
        if(State == PlayerState.Hooking) //Should move to another class
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            transform.Translate(Vector3.Normalize(target.position - transform.position) * Time.deltaTime * 50f, Space.World);
            if(Vector3.Distance(target.position,transform.position) <= 0.1f)
            {
                SetPlayerState(PlayerState.Normal);
                rb.gravityScale = 1f;
            }
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
        SetIsGrounded(Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround));
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
            // if(lastHorizontalDir != HorizontalMoveDir.Left)
            // {
            //     SetIsFacingRight(false);
            // }
            if(CheckLastHorizontalDirIs(HorizontalMoveDir.Left) && CanDash())
            {
                Dash((int)HorizontalMoveDir.Left);
            }
            lastHorizontalDir = HorizontalMoveDir.Left;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            // if(lastHorizontalDir != HorizontalMoveDir.Right)
            // {
            //     SetIsFacingRight(true);
            // }
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

        if(Input.GetKeyDown(KeyCode.Z)) //Should move to another class
        {
            int faceDir = IsFacingRight?1:-1;
            if((target.position.x - transform.position.x)*faceDir > 0)
                SetPlayerState(PlayerState.Hooking);
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
        State = newState;
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
        bool passStateCondition = (State == PlayerState.Normal);

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
    public void SetIsFacingRight(bool value)
    {
        if(State == PlayerState.Normal || State == PlayerState.Hooking)
        {
            if(IsFacingRight != value)
                Flip();
            IsFacingRight = value;
        }
    }
    private void SetCombatForm(Form form)
    {
        moveControl = Forms[(int)form];
        currentForm = form;
        for (int i = 0; i < Forms.Length; i ++)
        {
            if(i != (int)form)
                Forms[i].enabled = false;
        }
    }

    private void SetIsGrounded(bool value)
    {
        IsGrounded = value;
        if(State == PlayerState.Sliding && IsGrounded)
        {
            SetPlayerState(PlayerState.Normal);
        }
    }
    private void SetIsNextToWall(bool value)
    {
        lastFrameNextToWall = IsNextToWall;
        IsNextToWall = value;
        if(IsNextToWall != lastFrameNextToWall)
        {
            NotNextToWall();
        };
    }
}
