using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField] protected LayerMask platformLayerMask;
    protected bool isFacingRight = false;
    protected int moveDir = -1;
    [SerializeField] protected float moveSpeed;
    protected Rigidbody2D rb;

    protected const float ANTI_SLIDE_ON_FLOOR = 0.05f;
    protected const float MAX_FLOOR_SPEED = 2f;

    [SerializeField] protected float TURN_RESET_TIME = 2f;
    [SerializeField] protected float CHECK_DISTANCE = 0.6f;
    protected float lastTurn;
    [SerializeField] protected float pivotCheckDistanceX = 0.2f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void SetSpawnPoint(SpawnPoint newSpawnPoint)
    {
        base.SetSpawnPoint(newSpawnPoint);
    }

    protected override void Update()
    {
        base.Update();
        WallCheck();
        CheckFlip();
        if(isMoveable == false)
        {
            return;
        }
        Move();
    }
    protected void Move()
    {
        Debug.Log("Mực đẹp trai có gì sai");
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed * moveDir, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED), rb.velocity.y);
    }
    protected void WallCheck()
    {
        if(lastTurn + TURN_RESET_TIME < Time.time)
        {
            int facingValue = isFacingRight?1:-1;
            RaycastHit2D raycastHitB = Physics2D.Raycast(
                new Vector3(transform.position.x + pivotCheckDistanceX * facingValue, transform.position.y, transform.position.z), 
                Vector2.down, CHECK_DISTANCE, platformLayerMask);
            RaycastHit2D raycastHitH = Physics2D.Raycast(
                new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                Vector2.right * facingValue, CHECK_DISTANCE, platformLayerMask);
            bool checkResult = (raycastHitB.collider == null || raycastHitH.collider != null); //false => flip
            if(checkResult)
            {
                Flip();
            }
        }
    }
    protected void Flip()
    {
        moveDir *= -1;
        isFacingRight = !isFacingRight;
        lastTurn = Time.time;
    }

    protected void CheckFlip()
    {
        transform.rotation = isFacingRight ?Quaternion.Euler(0, 180, 0) : Quaternion.identity;
    }

    protected override void GetHitBehaviour(int _dmg)
    {
        base.GetHitBehaviour(_dmg);
        isMoveable = false;
        Invoke("ChangeMoveState", 1);
    }

    protected void ChangeMoveState()
    {
        isMoveable = !isMoveable; 
    }

    public override void AttackAction()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, new Vector2(moveDir, 0), attackRange, LayerMask.GetMask("Player"));
        if(hit.collider == null)
        {
            if(attackCountdown > 0)
            {
                return;
            }
            else
            {
                Flip();
            }
        }
        base.AttackAction();
    }
}
