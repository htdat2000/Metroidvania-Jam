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
            MoveControl = RedForm;
        }
        public void InitParam(GameObject _go, float _jumpForce)
        {
            NormalForm = (new GameObject("NormalForm")).AddComponent<NormalForm>();
            RedForm = (new GameObject("RedForm")).AddComponent<RedForm>();
            BlueForm = (new GameObject("BlueForm")).AddComponent<BlueForm>();
            YellowForm = (new GameObject("YellowForm")).AddComponent<YellowForm>();

            NormalForm.InitParam(_go, _jumpForce);
            RedForm.InitParam(_go, _jumpForce);
            BlueForm.InitParam(_go, _jumpForce);
            YellowForm.InitParam(_go, _jumpForce);

            MoveControl = RedForm;
        }
        public void Jump()
        {
            if(extraJump > 0)
            {
                MoveControl.Jump();
                extraJump--;
            }
        }
        public void Attack()
        {
            MoveControl.Attack();
        }
        public void Dash(int dir)
        {
            MoveControl.Dash(dir);
        }
        public void Charge()
        {
            MoveControl.Charge();
        }
        public void ExtraJumpRecover()
        {
            extraJump = 1; //will change base on player form
        }
        public void PlayerVelocityToZero()
        {
            MoveControl.VelocityToZero();
        }
        public void Slide()
        {
            MoveControl.Slide();
        }
        public void QuitSlide()
        {
            MoveControl.QuitSlide();
        }
    }
}
