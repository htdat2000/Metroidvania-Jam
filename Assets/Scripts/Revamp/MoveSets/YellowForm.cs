﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class YellowForm : MoveSet
    {
        // Start is called before the first frame update
        public YellowForm(GameObject _playerGO, float _jumpForce) : base(_playerGO, _jumpForce)
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
