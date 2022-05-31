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
    private GameObject objectHook;

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
            HookReturn(State.Returning);
        }
        else if((distance <= 0.5f))
        {
            if(currentState == State.Returning)
            {
                Deactive();
            }
            else if(currentState == State.Pulling)
            {
                Release();
            }
        }
    }

    void HookReturn(State _state)
    {
        dir = -dir;
        currentState = _state;
    }

    void Release()
    {
        if(objectHook != null)
        {
            objectHook.transform.SetParent(null);
            objectHook = null;
            Deactive();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Hookable") && (currentState == State.Free))
        {   
            currentState = State.Pulling;
            objectHook = col.gameObject;
            objectHook.transform.SetParent(this.gameObject.transform);
            HookReturn(State.Pulling);
        }
    }


}
