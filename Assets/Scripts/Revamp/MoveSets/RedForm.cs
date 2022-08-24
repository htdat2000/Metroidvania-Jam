using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class RedForm : MoveSet
    {
        public override void Slide()
        {
            // VelocityToZero();
            playerMover.SetPlayerState(PlayerMover.PlayerState.Sliding);
            playerRb.gravityScale = 0.75f;
        }
        public override void QuitSlide()
        {
            playerMover.SetPlayerState(PlayerMover.PlayerState.Normal);
            base.QuitSlide();
        }
    }
}
