﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void TakeDmg(int _dmg, GameObject attacker);
}
