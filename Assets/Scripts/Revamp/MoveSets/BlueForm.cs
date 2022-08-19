using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class BlueForm : MoveSet
    {
        // Start is called before the first frame update
        public BlueForm(GameObject _playerGo, float _jumpForce) : base(_playerGo, _jumpForce)
        {

        }
        void Start()
        {
            
        }
        // Update is called once per frame
        void Update()
        {
            
        }
        public virtual void Jump()
        {
            playerRb.velocity = Vector2.up * jumpForce/2f;
        }
    }
}

