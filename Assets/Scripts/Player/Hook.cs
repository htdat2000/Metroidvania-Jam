using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private float speed = 5;
    [SerializeField] private float limitDistance;
    
    enum State
    {
        Free,
        Returning,
        Pulling
    }
    private State currentState = State.Free;

    private Vector3 dir;

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
        CheckDistance(this.gameObject.transform.position);
    }

    public void PushHook(bool _isFacingRight)
    {
        this.gameObject.transform.rotation = _isFacingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        if(_isFacingRight)
        {
            dir = new Vector3 (1, 0, 0);
        }
        else
        {
            dir = new Vector3 (-1, 0, 0);
        }
    }

    public void Deactive()
    {
        this.gameObject.SetActive(false);
        currentState = State.Free;
        this.gameObject.transform.position = parent.transform.position;
        parent.gameObject.GetComponent<PlayerController>().BackToNormal();
    }

    void CheckDistance(Vector3 currentPos)
    {
        float distance = Vector3.Magnitude(currentPos - parent.position);
        if((distance >= limitDistance) && (currentState == State.Free))
        {
            HookReturn();
        }
        else if((distance <= 0.1f) && (currentState != State.Free))
        {
            Deactive();
        }
    }

    void HookReturn()
    {
        dir = -dir;
        currentState = State.Returning;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
    }


}
