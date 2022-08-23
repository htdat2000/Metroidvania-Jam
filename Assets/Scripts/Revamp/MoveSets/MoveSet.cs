using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class MoveSet : MonoBehaviour
    //This class is a base of all Color Move Set.
    {
        protected Rigidbody2D playerRb;
        protected float jumpForce;
        protected GameObject playerGO;
        protected PlayerMover playerMover;
        public MoveSet(GameObject _playerGO, float _jumpForce)
        {
            this.playerGO = _playerGO;
            this.playerRb = _playerGO.GetComponent<Rigidbody2D>();
            this.playerMover = _playerGO.GetComponent<PlayerMover>();
            this.jumpForce = _jumpForce;
        }
        public void InitParam(GameObject _playerGO, float _jumpForce)
        {
            this.playerGO = _playerGO;
            this.playerRb = _playerGO.GetComponent<Rigidbody2D>();
            this.playerMover = _playerGO.GetComponent<PlayerMover>();
            this.jumpForce = _jumpForce;
        }
        protected void Start()
        {
            
        }

        // Update is called once per frame
        protected void Update()
        {
            
        }
        public virtual void Jump()
        {
            playerRb.velocity = Vector2.up * jumpForce;
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
        protected virtual IEnumerator BackToNormal(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            playerMover.BackToNormal();
        }
    }
}
