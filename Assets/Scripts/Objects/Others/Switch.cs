using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Switch : MonoBehaviour, IDamageable
{
    protected bool _state = false;
    public bool state { get {return _state;} set{ _state = value;} }
    public UnityEvent action;

    public void TakeDmg(int _dmg, GameObject attacker)
    {
        if(attacker.CompareTag("Player"))
        {
            action?.Invoke();      
        }
    }
}
