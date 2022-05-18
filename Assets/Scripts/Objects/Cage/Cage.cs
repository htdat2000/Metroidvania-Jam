using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] GameObject soul;
    public bool canInteract = false;
    GameObject noti;
    public int skillIndex;
    void Start() {
        noti = GetComponentInChildren<NotiObject>().gameObject;
    }
    void Update() {
        if(Input.GetKey("c"))
        {
            Debug.Log("Cage: Pick up");
            soul.SetActive(false);
            noti.SetActive(false);
            CustomEvents.OnPlayerUnlock?.Invoke(skillIndex);
        }    
    }
}
