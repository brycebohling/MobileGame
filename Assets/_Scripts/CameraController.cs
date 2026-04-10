using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Cam { get; private set; }
    CinemachineCamera cam;

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

        cam = GetComponent<CinemachineCamera>();
        noisePerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
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
        noisePerlin.AmplitudeGain = amplitudeGain;
        noisePerlin.FrequencyGain = frequencyGain;
        shakeTime = time;
        shakeCounter = 0;
        isShaking = true;
    }

    void CameraStopShake()
    {
        noisePerlin.AmplitudeGain = 0f;
        noisePerlin.FrequencyGain = 0f;
        isShaking = false;
    }
}
