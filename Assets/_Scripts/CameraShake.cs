using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Cam { get; private set; }
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noisePerlin;
    bool isShaking = false;
    float shakeTime;
    float shakeCounter;


    void Awake() 
    {
        if (Cam != null && Cam != this)
        {
            Debug.Log("More than one CameraShake in a scene!");
        } else
        {
            Cam = this;
        }

        vcam = GetComponent<CinemachineVirtualCamera>();
        noisePerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (isShaking)
        {
            shakeCounter += Time.deltaTime;

            if (shakeCounter >= shakeTime)
            {
                CameraStopShake();
            }
        }
    }

    public void CameraStartShake(float AmplitudeGain, float FrequencyGain, float time)
    {
        noisePerlin.m_AmplitudeGain = AmplitudeGain;
        noisePerlin.m_FrequencyGain = FrequencyGain;
        shakeTime = time;
        shakeCounter = 0;
        isShaking = true;
    }

    void CameraStopShake()
    {
        noisePerlin.m_AmplitudeGain = 0f;
        noisePerlin.m_FrequencyGain = 0f;
        isShaking = false;
    }

}
