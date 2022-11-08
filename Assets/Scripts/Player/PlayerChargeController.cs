using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerChargeController : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private const float CHARGE_FULL_VALUE = 2f;

    private float chargeValue = 0;

    [SerializeField] Slider chargeCounter;
    
    public void ChargeCheck()
    {
        if(Input.GetKey(KeyCode.A) || CrossPlatformInputManager.GetButton("Charge"))
        { 
            player.IsCharging = true;
            chargeValue += Time.deltaTime;
            player.mainVCam.ZoomIn();
            player.mainVCam.ShakeCamera(2, 0.1f);
            DebugSetChargeCounter();
            player.DoHoldChargeBehavior();
        }
        if(Input.GetKeyUp(KeyCode.A) || CrossPlatformInputManager.GetButtonUp("Charge"))
        {
            if(chargeValue >= CHARGE_FULL_VALUE)
            {
                DoChargeAction();
            }
            player.mainVCam.ZoomOut();
            chargeValue = 0;
            DebugSetChargeCounter();
            player.IsCharging = false;
        }
    }
    public void DoChargeAction()
    {
        if(!player.IsSliding())
            player.SetYMotion(30f);
        else
        {
            int flyDir = player.IsFacingRight ? -1 : 1;
            player.SetXMotion(50f * flyDir);
            player.SetYMotion(5f);
        }
    }
    private void DebugSetChargeCounter()
    {
        chargeCounter.value = (chargeValue/CHARGE_FULL_VALUE);
    }
}
