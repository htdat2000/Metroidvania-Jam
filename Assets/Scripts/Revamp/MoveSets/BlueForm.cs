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
        public override void Jump()
        {
            playerRb.velocity = Vector2.up * jumpForce/2f;
        }
    }
}

