using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    static public EffectPool Instance;
    [SerializeField] private GameObject LandingEffect;
    [SerializeField] private int LandingEffectAmount;
    private GameObject[] LandingEffects;
    
    [SerializeField] private GameObject HitEffect;
    [SerializeField] private int HitEffectAmount;
    private GameObject[] HitEffects;

    [SerializeField] private GameObject JumpEffect;
    [SerializeField] private int JumpEffectAmount;
    private GameObject[] JumpEffects;

    [SerializeField] private GameObject SaveEffect;
    [SerializeField] private int SaveEffectAmount;
    private GameObject[] SaveEffects;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
            Debug.LogWarning("EffectPool: There are 2 instance of EffectPool");
        Instance = this;
        CreateLandingEffects();
        CreateHitEffects();
        CreateJumpEffects();
        CreateSaveEffects();
    }
    void CreateLandingEffects()
    {
        LandingEffects = new GameObject[LandingEffectAmount];
        for(int i = 0; i < LandingEffectAmount; i++)
        {
            LandingEffects[i] =  Instantiate(LandingEffect, Vector3.zero, Quaternion.identity, this.transform);
            LandingEffects[i].SetActive(false);
        }
    }
    public void GetLandingEffectInPool(Vector3 spawnposition)
    {
        for(int i = 0; i < LandingEffects.Length; i++)
        {
            if(!LandingEffects[i].activeSelf)
            {
                LandingEffects[i].transform.position = spawnposition;
                LandingEffects[i].SetActive(true);
                return;
            }
        }
    }
    void CreateHitEffects()
    {
        HitEffects = new GameObject[HitEffectAmount];
        for(int i = 0; i < HitEffectAmount; i++)
        {
            HitEffects[i] =  Instantiate(HitEffect, Vector3.zero, Quaternion.identity, this.transform);
            HitEffects[i].SetActive(false);
        }
    }
    public void GetHitEffectInPool(Vector3 spawnposition)
    {
        for(int i = 0; i < HitEffects.Length; i++)
        {
            if(!HitEffects[i].activeSelf)
            {
                HitEffects[i].transform.position = spawnposition;
                HitEffects[i].SetActive(true);
                return;
            }
        }
    }
    void CreateJumpEffects()
    {
        JumpEffects = new GameObject[JumpEffectAmount];
        for(int i = 0; i < JumpEffectAmount; i++)
        {
            JumpEffects[i] =  Instantiate(JumpEffect, Vector3.zero, Quaternion.identity, this.transform);
            JumpEffects[i].SetActive(false);
        }
    }
    public void GetJumpEffectInPool(Vector3 spawnposition)
    {
        for(int i = 0; i < JumpEffects.Length; i++)
        {
            if(!JumpEffects[i].activeSelf)
            {
                JumpEffects[i].transform.position = spawnposition;
                JumpEffects[i].SetActive(true);
                return;
            }
        }
    }
    void CreateSaveEffects()
    {
        SaveEffects = new GameObject[SaveEffectAmount];
        for(int i = 0; i < SaveEffectAmount; i++)
        {
            SaveEffects[i] =  Instantiate(SaveEffect, Vector3.zero, Quaternion.identity, this.transform);
            SaveEffects[i].SetActive(false);
        }
    }
    public void GetSaveEffectInPool(Vector3 spawnposition)
    {
        for(int i = 0; i < SaveEffects.Length; i++)
        {
            if(!SaveEffects[i].activeSelf)
            {
                SaveEffects[i].transform.position = spawnposition;
                SaveEffects[i].SetActive(true);
                return;
            }
        }
    }
}
