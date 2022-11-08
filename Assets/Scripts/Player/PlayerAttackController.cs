using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerAttackController : MonoBehaviour
{
    enum AttackStyle
    {
        Rapid,
        Interupt
    }
    [SerializeField] private PlayerController player;

    //Debug side
    [SerializeField] private Slider attackSilderDisplay;
    [SerializeField] private Image attackSilderDisplayFill;
    [SerializeField] private Text attackTextDisplay;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color middleColor;
    [SerializeField] private Color additionalColor;

    private const float ATTACK_TIME = 1f;
    private float attackTimeCount;

    private const float INTERUPT_COMBO = 0.3f;
    private const float END_ATTACK_PHASE = 0.6f;
    
    private int rapidIndex = 0;
    private int interuptIndex = 0;

    private int airRapidIndex = 0;
    private int airInteruptIndex = 0;

    private bool lastInAir = false;

    [SerializeField] private Animator anim;
    [SerializeField] private float firstAirAtkExtraY;

    [SerializeField] private Slider attackChargeSlider;
    [SerializeField] private float MAX_HOLD_TIME;
    private float holdTime = 0;
    void Update()
    {
        if(attackTimeCount > 0)
        {
            attackTimeCount -= Time.deltaTime;
            if(attackTimeCount <= END_ATTACK_PHASE)
            {
                player.IsAttacking = false;
            }
            UpdateDebugSlider(attackTimeCount);
            if(attackTimeCount <= 0)
            {
                UpdateDebugAttackName("Idle", normalColor);
            }
        }
    }

    public void AttackCheck()
    {
        if(Input.GetKeyDown(KeyCode.C) || CrossPlatformInputManager.GetButtonDown("Attack"))
        {
            CheckFlip();

            if(player.IsSliding())
            {
                DoSlidingAttack();
            }
            else if(player.IsDashing())
            {
                DoDashingAttack();
            }
            else if(player.GetGrounded())
            {
                if(attackTimeCount <= 0 || lastInAir == true)
                {   
                    DoFirstAttack();
                }
                else if(attackTimeCount < INTERUPT_COMBO)
                {
                    DoInteruptCombo();
                }
                else
                {
                    DoCombo();
                }
                lastInAir = false;
            }
            else
            {
                if(attackTimeCount <= 0 || lastInAir == false)
                {   
                    DoAirFirstAttack();
                }
                else if(attackTimeCount < INTERUPT_COMBO)
                {
                    DoAirInteruptCombo();
                }
                else
                {
                    DoAirCombo();
                }
                lastInAir = true;
            }
            player.IsAttacking = true;
            attackTimeCount = ATTACK_TIME;
            UpdateDebugSlider(attackTimeCount);
        }
        if(Input.GetKey(KeyCode.C) || CrossPlatformInputManager.GetButton("Attack"))
        {
            if(holdTime < MAX_HOLD_TIME)
            {
                holdTime += Time.deltaTime;
                DebugAtkCharge();
            }
        }
        if(Input.GetKeyUp(KeyCode.C) || CrossPlatformInputManager.GetButtonUp("Attack"))
        {
            if(holdTime >= MAX_HOLD_TIME)
            {
                DoChargeAttack();
            }
            holdTime = 0;
            DebugAtkCharge();
        }
    }
    private void CheckFlip()
    {
        if(player.GetInputVector().x < 0 && player.IsFacingRight)
        {
            player.Flip(false);
        }
        else if(player.GetInputVector().x > 0 && !player.IsFacingRight)
        {
            player.Flip(true);
        }
    }
    private void UpdateDebugSlider(float value)
    {
        attackSilderDisplay.value = value;
        if(value < INTERUPT_COMBO)
        {
            attackSilderDisplayFill.color = additionalColor;
        }
        else if(value < END_ATTACK_PHASE)
        {
            attackSilderDisplayFill.color = middleColor;
        }
        else
            attackSilderDisplayFill.color = normalColor;
    }
    private void UpdateDebugAttackName(string debugTxt, Color color)
    {
        attackTextDisplay.text = debugTxt;
        attackTextDisplay.color = color;
    }
    private void DoFirstAttack() //Real First Attack
    {
        rapidIndex = 0;
        interuptIndex = 0;
        airRapidIndex = 0;
        airInteruptIndex = 0;
        PlayAnim("Atk");
        UpdateDebugAttackName("1st", normalColor);
    }
    private void DoCombo()
    {
        if(rapidIndex <= 2)
        {
            rapidIndex ++;
            PlayAnim("Rapid" + rapidIndex);
            UpdateDebugAttackName("Rapid" + rapidIndex, middleColor);
            interuptIndex = 0;
        }
        else
            DoFirstAttack();
    }
    private void DoInteruptCombo()
    {
        if(interuptIndex <= 1)
        {
            interuptIndex ++;
            if(rapidIndex == 2) //hardcode
            {
                interuptIndex = 2;
            }
            PlayAnim("Inter" + interuptIndex);
            UpdateDebugAttackName("Inter" + interuptIndex, additionalColor);
            rapidIndex = 0;
        }
        else
            DoFirstAttack();
    }



    private void DoAirFirstAttack() //Real First Attack
    {
        airRapidIndex = 0;
        airInteruptIndex = 0;
        rapidIndex = 0;
        interuptIndex = 0;

        player.SetYMotion(firstAirAtkExtraY);

        PlayAnim("AirAtk");
        UpdateDebugAttackName("1st Air", normalColor);
    }
    private void DoAirCombo()
    {
        if(airRapidIndex <= 2)
        {
            airRapidIndex ++;
            PlayAnim("AirRapid"+airRapidIndex);
            UpdateDebugAttackName("Rapid Air" + airRapidIndex, middleColor);
            airInteruptIndex = 0;
        }
        else
            DoAirFirstAttack();
    }
    private void DoAirInteruptCombo()
    {
        if(airInteruptIndex <= 1)
        {
            airInteruptIndex ++;
            if(airRapidIndex == 2) //hardcode
            {
                airInteruptIndex = 2;
            }
            PlayAnim("AirInter"+airInteruptIndex);
            UpdateDebugAttackName("Inter Air" + airInteruptIndex, additionalColor);
            airRapidIndex = 0;
        }
        else
            DoAirFirstAttack();
    }
    private void PlayAnim(string animStr)
    {
        player.PlayAnim(animStr);
    }

    private void DoSlidingAttack() //Real First Attack
    {
        rapidIndex = 0;
        interuptIndex = 0;
        airRapidIndex = 0;
        airInteruptIndex = 0;

        PlayAnim("SlidingAtk");
        UpdateDebugAttackName("SliAtk", normalColor);
    }
    private void DoDashingAttack()
    {
        rapidIndex = 0;
        interuptIndex = 0;
        airRapidIndex = 0;
        airInteruptIndex = 0;

        PlayAnim("DashingAtk");
        UpdateDebugAttackName("DasAtk", normalColor);
    }
    private void DoChargeAttack()
    {
        rapidIndex = 0;
        interuptIndex = 0;
        airRapidIndex = 0;
        airInteruptIndex = 0;

        PlayAnim("ChargeAtk");
        UpdateDebugAttackName("ChargeAtk", normalColor);
    }

    public void SetLastInAir(bool value)
    {
        lastInAir = value;
    }
    private void DebugAtkCharge()
    {
        attackChargeSlider.value = (holdTime / MAX_HOLD_TIME);
    }
}
