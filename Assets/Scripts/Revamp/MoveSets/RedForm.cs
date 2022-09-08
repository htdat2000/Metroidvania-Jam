using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class RedForm : MoveSet
    {
        [SerializeField] private GameObject[] attackPrefabs;
        [SerializeField] private float alterComboTime;
        private bool wasAlterCombo = false;
        public override void InitParam(GameObject _playerGO)
        {
            base.InitParam(_playerGO);
            //Will use Scriptable
            defaultExtraJump = 1;
            attackComboIndex = 0;
            maxCombo = 3;
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Move()
        {
            base.Move();
            if(playerMover.State == PlayerMover.PlayerState.Hooking) //Should move to another class
            {
                playerRb.velocity = Vector2.zero;
                playerRb.gravityScale = 0f;
                playerTransform.Translate(Vector3.Normalize(target.position - playerTransform.position) * Time.deltaTime * 50f, Space.World);
                if(Vector3.Distance(target.position,playerTransform.position) <= 1f)
                {
                    playerMover.SetPlayerState(PlayerMover.PlayerState.Normal);
                    playerRb.gravityScale = 1f;
                }
            }
        }
        public override void Jump()
        {
            if(playerMover.State == PlayerMover.PlayerState.Sliding)
            {
                int jumpDir = playerMover.IsFacingRight ? -1 : 1;
                playerRb.AddForce(new Vector2(jumpDir * jumpForce/2f, jumpForce));
            }
            else if(playerMover.IsGrounded || extraJump > 0)
            {
                playerRb.AddForce(new Vector2(0f, jumpForce));
                if(!playerMover.IsGrounded)
                {
                    extraJump --;
                }
            }
        }
        public override void Dash(int dir)
        {
            playerRb.velocity = Vector2.right * dashAmount * dir;
            StartCoroutine(BackToNormal(dashTime));
        }
        public override void Attack()
        {
            if(lastAttackInput + attackTimeCombo <= Time.time)
            {
                attackComboIndex = 0;
            }
            else if((lastAttackInput + alterComboTime <= Time.time))
            {
                wasAlterCombo = true;
                attackComboIndex = 3;
            }
            playerAnim.Play("Attack" + (attackComboIndex + 1));
            UpdateLastAttackInput();
            base.Attack();
            if(attackComboIndex == 0)
            {
                wasAlterCombo = false;
            } 
        }
        public override void Slide()
        {
            playerMover.SetPlayerState(PlayerMover.PlayerState.Sliding);
            playerRb.gravityScale = 0.5f;
        }
        public override void QuitSlide()
        {
            playerMover.SetPlayerState(PlayerMover.PlayerState.Normal);
            base.QuitSlide();
        }
    }
}
