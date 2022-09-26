using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "newMusic", menuName = "Sound/Music")]
public class Music : Sound
{
    public int musicIndex; //musicIndex == scene build index in build setting
    public AudioClip audioClip;
}