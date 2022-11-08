using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Movable : MonoBehaviour
{
    protected enum State
    {
        Normal,
        Hurting,
        Dashing,
        Hooking,
        Charging,
        Sliding
    }
    protected float acceleration;
    protected float maxSpeed;
    protected float friction;
    protected float gravity;
    protected float jumpForce;
    protected int HP;
    //-move to scriptable
    [SerializeField] protected float MAX_STUN_TIME = 0.5f;
    protected float stunCounter = 0f;

    [HideInInspector] public bool IsFacingRight;

    [SerializeField] protected MovableConfig config;

    protected Vector2 motionVector;
    protected Vector2 horizontalVector = Vector2.left;

    protected Rigidbody2D rb;
    protected bool isGrounded;

    protected State state = State.Normal;

    public Animator anim;

    public bool IsAttacking;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetParam();
    }
    protected void ResetParam()
    {
        config.MapConfig(out acceleration, out maxSpeed, out friction, out gravity, out jumpForce, out HP);
    }
    protected virtual void ApplyGravity()
    {
        if (!isGrounded)
        {
            motionVector.y -= gravity * Time.deltaTime;
            motionVector.y = Mathf.Max(motionVector.y, -2 * jumpForce);
        }
    }
    protected virtual void Move()
    {
        rb.velocity = motionVector;
    }
    public virtual void SetGrounded(bool isGrounded)
    {
        if (!this.isGrounded && isGrounded)
        {
            motionVector.y = 0f;
        }
        this.isGrounded = isGrounded;
    }
    protected virtual void Dead()
    {

    }
    protected virtual void TakeDmg(int dmg)
    {
        if(HP <= 0)
        {
            Dead();
            return;
        }
        HP -= dmg;
    }
    public void PlayAnim(string animStr)
    {
        anim.Play(animStr);
    }
    public void Flip(bool right)
    {
        if (right)
        {
            transform.localScale = Vector3.one;
            IsFacingRight = true;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            IsFacingRight = false;
        }
    }
    public bool IsNormal()
    {
        return state == State.Normal;
    }
    public virtual void GetHit(Vector2 forceVector, bool isRightOfPlayer, int dmg = 1)
    {
        int horizontalForceDir = isRightOfPlayer ? 1 : -1;
        motionVector = forceVector;
        motionVector.x *= horizontalForceDir;
        state = State.Hurting;
        stunCounter = MAX_STUN_TIME;
        PlayAnim("Hurt");
        TakeDmg(dmg);
    }
}
