using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamerasShake : MonoBehaviour
{
    private CinemachineVirtualCamera cvm;
    private float shakeTimer;
    CinemachineBasicMultiChannelPerlin cbcp;
    // Start is called before the first frame update
    void Start()
    {
        cvm = GetComponent<CinemachineVirtualCamera>();
        cbcp = cvm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CustomEvents.OnScreenShakeDanger += Shake;
    }
    void OnDestroy()
    {
        CustomEvents.OnScreenShakeDanger -= Shake;
    }

    // Update is called once per frame
    void Shake(float amount, float time)
    {
        // cbcp.m_AmplitudeGain = amount;
        cbcp.m_FrequencyGain = amount;
        shakeTimer = time;
        Debug.Log("CamerasShake: shake");
    }

    void Update() {
        if(shakeTimer >= 0f)
        {
            shakeTimer -= Time.deltaTime;    
            if(shakeTimer <= 0f)    
                cbcp.m_FrequencyGain = 0;
        }
    }
}
