using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise; 

    // Shake parameters
    public float shakeIntensity = 1.2f;
    public float shakeDuration = 0.5f;
    public bool deges = false;
    void Start()
    {
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void Update()
    {

            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain= Mathf.Lerp(noise.m_AmplitudeGain, 0, Time.deltaTime*3);
        
       
    }

    public void ShakeScreen()
    {
        if (noise != null)
        {
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeIntensity;
            StartCoroutine("StopShaking");
        }
    }

    IEnumerator StopShaking()
    {
        deges = true;
        yield return new WaitForSeconds(shakeDuration);
        deges = false;
      
    }
}
