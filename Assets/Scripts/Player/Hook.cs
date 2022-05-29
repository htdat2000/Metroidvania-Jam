using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private float speed = 5;
    private Vector3 dir;

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
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
        this.gameObject.transform.position = parent.transform.position;
    }
}
