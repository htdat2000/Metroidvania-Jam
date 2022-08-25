using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class RedForm : MoveSet
    {
        [SerializeField] private GameObject[] attackPrefabs;
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
            UpdateLastAttackInput();
            playerAnim.Play("Attack" + (attackComboIndex + 1));
            Debug.Log("Attack index" + attackComboIndex);
            base.Attack(); 
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
