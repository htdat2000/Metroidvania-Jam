using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerTargetChecker targetChecker;
    private void Update() 
    {
        target = targetChecker.GetNearestTarget();
        if(target != null)
        {
            if(!line.gameObject.activeSelf)
                line.gameObject.SetActive(true);
            line.positionCount = 2;
            line.SetPosition(0, player.position);
            line.SetPosition(1, target.position);
        }
        else
        {
            if(line.gameObject.activeSelf)
                line.gameObject.SetActive(false);
        }
    }
}
