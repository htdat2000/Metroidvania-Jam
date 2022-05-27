using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static bool[] isColorActive = new bool[7] {true, true, true, false, false, false, false};
    //                                                white  red    blue   yel    vio    ora    gre

    void Start() {
        CustomEvents.OnPlayerUnlock += UnlockSkill;
    }
    void OnDestroy() {
        CustomEvents.OnPlayerUnlock -= UnlockSkill;
    }
    void UnlockSkill(int index)
    {
        isColorActive[index] = true;
        Debug.Log("PlayerData: unlocked " + isColorActive);
    }
}
