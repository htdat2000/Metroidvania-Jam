using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool[] unlockSkill = new bool[6] {false, false, false, false, false, false};
    //                                        red    blue   yel    vio    ora    gre

    void Start() {
        CustomEvents.OnPlayerUnlock += UnlockSkill;
    }
    void OnDestroy() {
        CustomEvents.OnPlayerUnlock -= UnlockSkill;
    }
    void UnlockSkill(int index)
    {
        unlockSkill[index] = true;
        Debug.Log("PlayerData: unlocked " + unlockSkill);
    }
}
