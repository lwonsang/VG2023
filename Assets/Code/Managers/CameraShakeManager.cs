using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance { get; private set; }

    private CinemachineBasicMultiChannelPerlin shake;
    private float shakeTimer;

    private IEnumerator shaketime;

    private void Awake()
    {
        Instance = this;
        shake = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float shakeStrength, float time)
    {
        Debug.Log("snom");
        shake.m_AmplitudeGain = shakeStrength;
        if(shaketime != null )
        {
            StopCoroutine( shaketime );
        }
        shaketime = ShakeCoroutine(time);
        StartCoroutine(shaketime);
    }

    public IEnumerator ShakeCoroutine(float shakeTimer)
    {
        for(int i = 0; i < shakeTimer; i++)
        {
            yield return null;
        }
        shake.m_AmplitudeGain = 0f;
    }
}
