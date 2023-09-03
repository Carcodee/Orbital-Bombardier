using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;
    public CinemachineVirtualCamera cam;
    public CinemachineBasicMultiChannelPerlin noise;
    [SerializeField]
    private float _defaultAmplitude;
    [SerializeField]
    private float _defaultFrecuency;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    void Start()
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GenerateShake(1f,1f,1f);
        }
    }

    public void GenerateShake(float amplitude, float frecuency,float time)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frecuency;
        StartCoroutine(createDelay(time));

        

    }

    IEnumerator createDelay(float time)
    {
        yield return new WaitForSeconds(time);
        noise.m_AmplitudeGain = _defaultAmplitude;
        noise.m_FrequencyGain = _defaultFrecuency;
    }
}
