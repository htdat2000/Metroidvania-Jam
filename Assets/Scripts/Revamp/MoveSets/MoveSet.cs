using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class MoveSet : MonoBehaviour
    //This class is a base of all Color Move Set.
    {
        protected Rigidbody2D playerRb;
        [SerializeField] protected float jumpForce = 5f;
        protected GameObject playerGO;
        protected PlayerMover playerMover;
        protected float dashAmount = 50f;
        protected float dashTime = 0.5f;
        protected float extraJump = 1;
        protected float defaultExtraJump = 1;
        public void InitParam(GameObject _playerGO)
        {
            this.playerGO = _playerGO;
            this.playerRb = _playerGO.GetComponent<Rigidbody2D>();
            this.playerMover = _playerGO.GetComponent<PlayerMover>();
        }
        public virtual void Jump()
        {
            if(extraJump > 0)
            {
                playerRb.AddForce(new Vector2(0f, jumpForce));
            }
            extraJump-=1;
        }
        public virtual void Dash(int dir)
        {

        }
        public virtual void Attack()
        {
            playerRb.velocity = Vector2.up * jumpForce;
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
