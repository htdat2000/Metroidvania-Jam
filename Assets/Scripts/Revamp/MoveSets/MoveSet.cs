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
        public MoveSet(Rigidbody2D _playerRb, float _jumpForce)
        {
            this.playerRb = _playerRb;
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
        public virtual void Dash()
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }
        public virtual void Attack()
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }
        public virtual void Charge()
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }
    }
}
