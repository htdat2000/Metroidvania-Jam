using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private PlayerController parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<PlayerController>();
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            parent.isOnGround = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            parent.isOnGround = false;
        }
    }
}
