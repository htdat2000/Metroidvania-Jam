using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : Movable
{
    //---------
    [SerializeField] private float MAX_CHANGE_DIR_TIME = 1.5f;
    [SerializeField] private float MIN_CHANGE_DIR_TIME = 0.5f;
    private float changeDirCounter = 0;
    //--------
    [SerializeField] private Transform spawnPoint;
    //--------
    [SerializeField] private EnemiesEyes attackController;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
            if(changeDirCounter >= 0)
            {
                changeDirCounter -= Time.deltaTime;
            }
            horizontalVector = GetHorizontalVector();
            ApplyHorizontalForce();
            break;
            case State.Hurting:
            stunCounter -= Time.deltaTime;
            if(stunCounter <= 0)
            {
                state = State.Normal;
            }
            break;
        }
        ApplyFriction();
        ApplyGravity();
        Move();
    }
    private Vector2 GetHorizontalVector()
    {
        if(changeDirCounter <= 0 && !IsAttacking)
        {
            horizontalVector.x *= -1;
            changeDirCounter = UnityEngine.Random.Range(MIN_CHANGE_DIR_TIME, MAX_CHANGE_DIR_TIME);
            if(horizontalVector.x < 0)
            {
                Flip(false);
            }
            else
                Flip(true);
        }
        return horizontalVector;
    }
    protected override void ApplyGravity()
    {
        base.ApplyGravity();
    }
    private void ApplyHorizontalForce()
    {
        if(horizontalVector != Vector2.zero)
        {
            motionVector.x += horizontalVector.x * acceleration * Time.deltaTime;
            motionVector.x = Mathf.Clamp(motionVector.x, -1 * maxSpeed, maxSpeed);
        }
    }

    private void ApplyFriction()
    {
        if(horizontalVector == Vector2.zero)
        {
            if(isGrounded)
                motionVector.x = Mathf.Lerp(motionVector.x, 0, friction);
            else
                motionVector.x = Mathf.Lerp(motionVector.x, 0, friction/100f);
        }
    }
    public override void GetHit(Vector2 forceVector, bool isRightOfPlayer, int dmg = 1)
    {
        base.GetHit(forceVector, isRightOfPlayer, dmg);
        attackController.ResetAttack();
    }
    protected override void Dead()
    {
        Respawn();
        Debug.Log("Enemy " + gameObject.name + " has dead");
    }
    private void Respawn() //Temp respawn function
    {
        ResetParam();
        this.transform.position = spawnPoint.position;
    }
}
