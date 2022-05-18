using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

public class NotiObject : MonoBehaviour
{
    [SerializeField] GameObject text; 
    Cage parent;
    // Start is called before the first frame update
    void Start() {
        parent = GetComponentInParent<Cage>();    
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            // parent.isNextToWall = true;
            text.SetActive(true);
            parent.canInteract = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            // parent.isNextToWall = false;
            text.SetActive(false);
            parent.canInteract = false;
        }
    }
}
