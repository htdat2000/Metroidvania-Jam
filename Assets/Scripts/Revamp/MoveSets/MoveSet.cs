using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class MoveSet : MonoBehaviour
    //This class is a base of all Color Move Set.
    {
        protected Rigidbody2D playerRb;
        protected GameObject playerGO;
        protected Transform playerTransform;
        protected PlayerMover playerMover;
        protected Animator playerAnim;
        [SerializeField] protected float jumpForce = 5f;
        [SerializeField] protected float dashAmount = 5f;
        [SerializeField] protected float dashTime = 0.5f;
        [SerializeField] protected float attackTimeCombo = 0.7f;
        [SerializeField] protected float defaultAttackInputCooldown = 0.5f;
        private float speed;
        private float attackInputCooldown;
        protected float extraJump = 1;
        protected float defaultExtraJump = 1;
        protected int attackComboIndex;
        protected int maxCombo;
        protected float lastAttackInput;
        protected bool wasInit = false;

        [SerializeField] protected Transform target;
        [SerializeField] private float MAX_SPEED = 50f;
        private float horizonMove;
        public virtual void InitParam(GameObject _playerGO)
        {
            this.playerGO = _playerGO;
            this.playerRb = _playerGO.GetComponent<Rigidbody2D>();
            this.playerMover = _playerGO.GetComponent<PlayerMover>();
            this.playerAnim = _playerGO.GetComponent<Animator>();
            this.speed = playerMover.speed;
            this.playerTransform = playerGO.transform;
            wasInit = true;
        }
        public virtual void Update()
        {
            if(wasInit)
            {
                HorizontalMoveCheck();
                if(attackInputCooldown <= 0)
                {
                    if(playerMover.State == PlayerMover.PlayerState.Attacking)
                    {
                        playerMover.SetPlayerState(PlayerMover.PlayerState.Normal);
                    }
                }
                else
                {
                    attackInputCooldown -= Time.deltaTime;
                }
            }
        }
        public virtual void Move()
        {
            if(playerMover.State == PlayerMover.PlayerState.Normal && IsReadHorizonInput())
            {
                playerRb.AddForce(new Vector2(horizonMove * speed * Time.deltaTime, 0f)); // can optimize
            }
        } 
        protected virtual bool IsReadHorizonInput()
        {
            if(playerRb.velocity.x * horizonMove < 0 || Mathf.Abs(playerRb.velocity.x) < MAX_SPEED)
            {
                return true;
            }
            return false;
        }
        protected virtual void HorizontalMoveCheck()
        {
            horizonMove = Input.GetAxisRaw("Horizontal");
        }
        public virtual void Jump()
        {
            if(playerMover.IsGrounded)
            {
                playerRb.AddForce(new Vector2(0f, jumpForce));
            }
        }
        public virtual void Dash(int dir)
        {
            return;
        }
        public virtual void Attack()
        {
            if(playerMover.State != PlayerMover.PlayerState.Hooking)
            {
                attackInputCooldown = defaultAttackInputCooldown;

                playerMover.SetPlayerState(PlayerMover.PlayerState.Attacking);
                attackComboIndex = (++attackComboIndex)%(maxCombo);
            }
        }
        protected void UpdateLastAttackInput()
        {
            lastAttackInput = Time.time;
        }
        public virtual void Charge()
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }
        public virtual void VelocityToZero()
        {
            playerRb.velocity = Vector2.zero;
        }
        protected virtual IEnumerator BackToNormal(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            playerMover.BackToNormal();
        }
        public virtual void Slide()
        {
            return;
        }
        public virtual void QuitSlide()
        {
            playerRb.gravityScale = 1f;
        }
        public virtual void ExtraJumpRecover()
        {
            extraJump = defaultExtraJump;
        }
    }
}
