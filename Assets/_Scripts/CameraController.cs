using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Cam { get; private set; }
    CinemachineVirtualCamera vcam;

    [Header("Camera Shake")]
    CinemachineBasicMultiChannelPerlin noisePerlin;
    bool isShaking = false;
    float shakeTime;
    float shakeCounter;

    [Header("Camera Zoom")]
    float baseCameraSize;
    float cameraZoomCounter;


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

    public void CameraStartShake(float amplitudeGain, float frequencyGain, float time)
    {
        noisePerlin.m_AmplitudeGain = amplitudeGain;
        noisePerlin.m_FrequencyGain = frequencyGain;
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
