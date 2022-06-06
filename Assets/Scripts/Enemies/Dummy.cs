﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
    [SerializeField] private LayerMask platformLayerMask;
    private bool isFacingRight = true;
    private int moveDir = 1;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;

    private const float ANTI_SLIDE_ON_FLOOR = 0.05f;
    private const float MAX_FLOOR_SPEED = 2f;

    [SerializeField] private float TURN_RESET_TIME = 2f;
    [SerializeField] private float CHECK_DISTANCE = 0.6f;
    private float lastTurn;
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

    void Update()
    {
        if(isMoveable == false)
        {
            return;
        }
        Move();
        WallCheck();
        CheckFlip();
    }
    void Move()
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime * moveSpeed * moveDir, -MAX_FLOOR_SPEED, MAX_FLOOR_SPEED), rb.velocity.y);
    }
    void WallCheck()
    {
        if(lastTurn + TURN_RESET_TIME < Time.time)
        {
            int facingValue = isFacingRight?1:-1;
            float pivotCheckDistanceX = 0.2f;
            RaycastHit2D raycastHitB = Physics2D.Raycast(
                new Vector3(transform.position.x + pivotCheckDistanceX * facingValue, transform.position.y, transform.position.z), 
                Vector2.down, CHECK_DISTANCE, platformLayerMask);
            RaycastHit2D raycastHitH = Physics2D.Raycast(
                new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                Vector2.right * facingValue, CHECK_DISTANCE, platformLayerMask);
            bool checkResult = (raycastHitB.collider == null || raycastHitH.collider != null); //false => flip
            if(checkResult)
            {
                moveDir *= -1;
                isFacingRight = !isFacingRight;
                lastTurn = Time.time;
            }
        }
    }

    void CheckFlip()
    {
        transform.rotation = isFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
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
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            IDamageable player = col.gameObject.GetComponent<IDamageable>();
            player.TakeDmg(dmg, this.gameObject);
            CustomEvents.OnScreenShakeDanger?.Invoke(GameConst.SHAKE_DANGER_AMOUNT, GameConst.SHAKE_DANGER_TIME);
            EffectPool.Instance.GetHitEffectInPool(col.gameObject.transform.position);
        }
    }
}
