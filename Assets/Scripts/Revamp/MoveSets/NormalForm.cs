using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class NormalForm : MoveSet
    {
        public override void Dash(int dir)
        {
            playerRb.AddForce(Vector2.right * dashAmount * dir * Time.deltaTime);
            StartCoroutine(BackToNormal(dashTime));
        }
    }
}
