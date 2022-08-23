using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class RedForm : MoveSet
    {
        // Start is called before the first frame update
        public RedForm(GameObject _playerGO, float _jumpForce) : base(_playerGO, _jumpForce)
        {

        }
        public override void Jump()
        {
            playerRb.velocity = Vector2.up * jumpForce/2f;
        }
    }
}
