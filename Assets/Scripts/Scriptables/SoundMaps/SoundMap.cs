using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Sound Map", menuName = "Sound Map")]
public class SoundMap : ScriptableObject
{
    public List<SoundAsset> items;
}
[Serializable]
public class SoundAsset
{
    public string name;
    public AudioClip audioClip;
}
