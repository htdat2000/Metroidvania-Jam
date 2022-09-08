using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "newSFX", menuName = "Sound/SFX")]
public class SFX : Sound
{
    [System.Serializable]
    public class SFXType
    {
        public SFXState type;
        public AudioClip clip;
    }

    public enum SFXState
    {
        HurtSFX,
        AttackSFX,
        DieSFX
    }
    public SFXType[] types;

    public void PlaySFX(SFXState state)
    {
        if(types.Length <= 0)
        {
            return;
        }
        SFXType _sfxType = Array.Find(types, type => type.type == state);
        if(_sfxType != null)
        {
            
            SFXManager.sfxManager.PlaySFX(_sfxType.clip);
        }    
    }
}