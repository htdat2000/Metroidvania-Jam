using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    static public EffectPool Instance;
    [SerializeField] private GameObject LandingEffect;
    [SerializeField] private int LandingEffectAmount;
    private GameObject[] LandingEffects;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
            Debug.LogWarning("EffectPool: There are 2 instance of EffectPool");
        Instance = this;
        CreateLandingEffects();
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
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetLandingEffectInPool(Vector3 spawnposition)
    {
        for(int i = 0; i < LandingEffects.Length; i++)
        {
            if(!LandingEffects[i].active)
            {
                LandingEffects[i].transform.position = spawnposition;
                LandingEffects[i].SetActive(true);
                return;
            }
        }
    }
}
