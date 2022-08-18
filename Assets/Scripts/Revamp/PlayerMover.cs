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

    private MoveController moveController;

    private MoveSet MoveControl;

    public enum Form
    {
        Normal,
        Red,
        Blue,
        Yellow
    }
    [SerializeField] private Form currentForm;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        moveController = new MoveController(rb, jumpForce);
        currentForm = Form.Normal;
    }
    private void Update()
    {
        HorizontalMoveCheck();
        GroundCheck();
        InputCheck();
    }
    private void FixedUpdate() 
    {
        rb.velocity = new Vector2(horizonMove * speed, rb.velocity.y);
    }
    private void HorizontalMoveCheck()
    {
        horizonMove = Input.GetAxisRaw("Horizontal");
    }
    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if(isGrounded)
            moveController.ExtraJumpRecover();
    }
    private void InputCheck()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveController.Jump(currentForm);
        }
    }
}
