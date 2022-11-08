using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetChecker : Checker
{
    private PlayerController player;
    private List<Transform> targetsInDistance;
    private Transform nearestTarget;

    private void Start()
    {
        targetsInDistance = new List<Transform>();
        player = obj.GetComponent<PlayerController>();
    }
    private void Update()
    {
        if(nearestTarget != null && !FaceCheck(nearestTarget))
            nearestTarget = null;
        if(targetsInDistance != null && targetsInDistance.Count > 0)
        {
            foreach(Transform target in targetsInDistance)
            {
                if(FaceCheck(target))
                {
                    if(nearestTarget == null)
                    {
                        nearestTarget = target;
                        return;
                    }
                    if(Vector3.Distance(target.position, transform.position) < Vector3.Distance(nearestTarget.position, transform.position))
                    {
                        nearestTarget = target;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        else
            nearestTarget = null;
    }
    private bool FaceCheck(Transform target)
    {
        return ((target.position.x > transform.position.x) && player.IsFacingRight) || ((target.position.x < transform.position.x) && !player.IsFacingRight);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("HookTarget") || other.CompareTag("Enemies"))
            targetsInDistance.Add(other.transform);
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("HookTarget") || other.CompareTag("Enemies"))
            targetsInDistance.Remove(other.transform);
    }
    public Transform GetNearestTarget()
    {
        return nearestTarget;
    }
}
