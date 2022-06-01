using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
    private bool isFacingRight = true;
    private int moveDir = 1;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;

    private const float ANTI_SLIDE_ON_FLOOR = 0.05f;
    private const float MAX_FLOOR_SPEED = 2f;

    //Debug
    private float turnReset = 3f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isMoveable);
        if(isMoveable == false)
        {
            return;
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed * moveDir, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED), rb.velocity.y);
        Turning();
        CheckFlip();
    }

    void CheckFlip()
    {
        transform.rotation = isFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    protected override void GetHitBehaviour(int _dmg)
    {
        base.GetHitBehaviour(_dmg);
        isMoveable = false;
        Invoke("ChangeMoveState", 0.1f);
    }

    protected void ChangeMoveState()
    {
        isMoveable = !isMoveable; 
    }

    protected void Turning()
    {
        turnReset -= Time.deltaTime;
        if(turnReset <= 0)
        {
            turnReset = 3f;
            moveDir *= -1;
            isFacingRight = !isFacingRight;
        }
    }
}
