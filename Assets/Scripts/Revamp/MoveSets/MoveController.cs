using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Player
{
    public class MoveController : MonoBehaviour
    //This class just has methods for handle player skills.
    {
        private NormalForm NormalForm;
        private RedForm RedForm;
        private YellowForm YellowForm;
        private BlueForm BlueForm;
        private int extraJump;

        private MoveSet MoveControl;
        
        public MoveController(Rigidbody2D _rb, float _jumpForce)
        {
            NormalForm = new NormalForm(_rb, _jumpForce);
            RedForm = new RedForm(_rb, _jumpForce);
            BlueForm = new BlueForm(_rb, _jumpForce);
            YellowForm = new YellowForm(_rb, _jumpForce);
        }
        public void Jump(PlayerMover.Form currentForm)
        {
            if(extraJump > 0)
            switch(currentForm)
            {
                case PlayerMover.Form.Normal:
                    NormalForm.Jump();
                break;
                case PlayerMover.Form.Red:
                    RedForm.Jump();
                break;
                case PlayerMover.Form.Blue:
                    BlueForm.Jump();
                break;
                case PlayerMover.Form.Yellow:
                    YellowForm.Jump();
                break;
            }
            extraJump--;
        }
        public void Attack(PlayerMover.Form currentForm)
        {
            switch(currentForm)
            {
                case PlayerMover.Form.Normal:
                    NormalForm.Attack();
                break;
                case PlayerMover.Form.Red:
                    RedForm.Attack();
                break;
                case PlayerMover.Form.Blue:
                    BlueForm.Attack();
                break;
                case PlayerMover.Form.Yellow:
                    YellowForm.Attack();
                break;
            }
        }
        public void Dash(PlayerMover.Form currentForm)
        {
            switch(currentForm)
            {
                case PlayerMover.Form.Normal:
                    NormalForm.Dash();
                break;
                case PlayerMover.Form.Red:
                    RedForm.Dash();
                break;
                case PlayerMover.Form.Blue:
                    BlueForm.Dash();
                break;
                case PlayerMover.Form.Yellow:
                    YellowForm.Dash();
                break;
            }
        }
        public void Charge(PlayerMover.Form currentForm)
        {
            switch(currentForm)
            {
                case PlayerMover.Form.Normal:
                    NormalForm.Charge();
                break;
                case PlayerMover.Form.Red:
                    RedForm.Charge();
                break;
                case PlayerMover.Form.Blue:
                    BlueForm.Charge();
                break;
                case PlayerMover.Form.Yellow:
                    YellowForm.Charge();
                break;
            }
        }
        public void ExtraJumpRecover()
        {
            extraJump = 1; //will change base on player form
        }
    }
}
