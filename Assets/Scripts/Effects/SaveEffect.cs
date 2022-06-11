using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HideMe()
    {
        gameObject.SetActive(false);
    }
    void OnEnable() {
        GetComponent<Animator>().Play("Save");
        GetComponentInChildren<ParticleSystem>().GetComponent<ParticleSystem>().Clear();
        GetComponentInChildren<ParticleSystem>().GetComponent<ParticleSystem>().Play();
    }
}
