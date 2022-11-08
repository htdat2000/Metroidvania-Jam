using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private float defaultAmplitude;
    private float defaultFrequency;

    private float shakeTime;
    private float shakeTimeTotal;

    private CinemachineVirtualCamera cvc;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    private float startIntensity;

    [SerializeField] float minZoomValue;
    [SerializeField] float maxZoomValue;

    private int zoomValue = 0;
    [SerializeField] float zoomSpeed = 3f;

    // /public LensSettings(float fov, float orthographicSize, float nearClip, float farClip, float dutch)
    // private 
    private void Awake() 
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
        cbmcp = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        defaultAmplitude = cbmcp.m_AmplitudeGain;
        defaultFrequency = cbmcp.m_FrequencyGain;
    }
    public void ShakeCamera(float intensity, float time)
    {
        cbmcp.m_AmplitudeGain = intensity;
        cbmcp.m_FrequencyGain = 1f;
        startIntensity = intensity;
        shakeTime = time;
        shakeTimeTotal = time;
    }
    public void ZoomIn()
    {
        zoomValue = -1;
    }
    public void ZoomOut()
    {
        zoomValue = 1;
    }
    private void Update()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if(shakeTime<=0)
            {
                cbmcp.m_AmplitudeGain = Mathf.Lerp(startIntensity, defaultAmplitude , 1 - (shakeTime/shakeTimeTotal));
                cbmcp.m_FrequencyGain = defaultFrequency;
            }
        }
        if(zoomValue != 0)
        {
            cvc.m_Lens.FieldOfView = Mathf.Clamp(cvc.m_Lens.FieldOfView + zoomValue*zoomSpeed*Time.deltaTime, maxZoomValue, minZoomValue);
        }
    }
}
