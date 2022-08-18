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
        
        public MoveController(GameObject _go, float _jumpForce)
        {
            NormalForm = new NormalForm(_go, _jumpForce);
            RedForm = new RedForm(_go, _jumpForce);
            BlueForm = new BlueForm(_go, _jumpForce);
            YellowForm = new YellowForm(_go, _jumpForce);
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
        public void Dash(PlayerMover.Form currentForm, int dir)
        {
            switch(currentForm)
            {
                case PlayerMover.Form.Normal:
                    NormalForm.Dash(dir);
                break;
                case PlayerMover.Form.Red:
                    RedForm.Dash(dir);
                break;
                case PlayerMover.Form.Blue:
                    BlueForm.Dash(dir);
                break;
                case PlayerMover.Form.Yellow:
                    YellowForm.Dash(dir);
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
