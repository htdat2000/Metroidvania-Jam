using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float knockUpForce = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col) {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * knockUpForce);
        Debug.Log("GetHit");
    }
}
