using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class NormalForm : MoveSet
    {
        //these val will config by Scriptable Object
        private float dashAmount = 50000f;
        private float dashTime = 0.5f;
        // Start is called before the first frame update
        public NormalForm(GameObject _playerRb, float _jumpForce) : base(_playerRb, _jumpForce)
        {

        }
        public override void Dash(int dir)
        {
            playerRb.velocity = Vector2.right * dashAmount * dir;
            Debug.Log(playerRb.gameObject.name);
            StartCoroutine(BackToNormal(dashTime));
        }
    }
}
