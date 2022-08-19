using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class NormalForm : MoveSet
    {
        //these val will config by Scriptable Object
        private float dashAmount = 20f;
        private float dashTime = 0.5f;
        // Start is called before the first frame update
        public NormalForm(GameObject _playerRb, float _jumpForce) : base(_playerRb, _jumpForce)
        {

        }
        void Start()
        {
            
        }
        // Update is called once per frame
        void Update()
        {
            
        }
        public override void Dash(int dir)
        {
            playerRb.velocity = Vector2.right * dashAmount * dir;
            // Invoke("BackToNormalIV", dashTime);
        }
        protected override IEnumerator BackToNormal(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            playerMover.BackToNormal();
        }
        protected void BackToNormalIV()
        {
            // playerMover.BackToNormal();
        }
    }
}
