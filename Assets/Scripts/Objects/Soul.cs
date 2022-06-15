using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] int hpValue = 2;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerData>().AddHp(hpValue);
            Destroy(this.gameObject);
        }
    }
}
