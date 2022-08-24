using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class BlueForm : MoveSet
    {
        public override void Jump()
        {
            playerRb.velocity = Vector2.up * jumpForce/2f;
        }
    }
}

