using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] float changeTime;
    private Vector3 punchScale = new Vector3(1.05f, 1.05f, 1f);
    public int playerMaxHP = 10;
    void Init()
    {
        CustomEvents.OnHPChange += OnHPChange;
        CustomEvents.OnMaxHPChange += OnMaxHPChange;

        // playerMaxHP = PlayerData.Instance.defaultHP;
        slider = GetComponent<Slider>();
    }
    void OnDestroy()
    {
        CustomEvents.OnHPChange -= OnHPChange;
        CustomEvents.OnMaxHPChange -= OnMaxHPChange;
    }
    void Start()
    {
        Init();
    }
    void OnHPChange(int value)
    {
        slider.DOValue( (float)value / playerMaxHP , changeTime);
        transform.DOPunchScale(punchScale, changeTime, 1, 0.5f);
    }
    void OnMaxHPChange(int value)
    {
        playerMaxHP = value;
    }
}
