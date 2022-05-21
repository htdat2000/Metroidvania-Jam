using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorTrigger : MonoBehaviour
{
    private GameObject text; 
    void Start()
    {
        text = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            text.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            // text.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
