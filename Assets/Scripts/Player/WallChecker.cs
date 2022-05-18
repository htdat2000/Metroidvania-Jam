using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
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
            parent.isNextToWall = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Ground"))
        {
            parent.isNextToWall = false;
        }
    }
}
