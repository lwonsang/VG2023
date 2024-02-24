using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance { get; private set; }

    private CinemachineBasicMultiChannelPerlin shake;

    private IEnumerator shaketime;

    private void Awake()
    {
        Instance = this;
        shake = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float shakeStrength, float time)
    {
        
        
        if (shaketime != null )
        {
            StopCoroutine(shaketime);
            shaketime = null;
        }
        if (time <= 0)
        {
            return;
        }
        if (shakeStrength == float.NaN)
            return;
        Debug.Log("aeiou" + shakeStrength);
        shake.m_AmplitudeGain = shakeStrength;
        shaketime = ShakeCoroutine(time, shakeStrength);
        StartCoroutine(shaketime);
    }

    public IEnumerator ShakeCoroutine(float shakeTimer, float shakeStrength)
    {
        for(int i = 0; i < shakeTimer; i++)
        {
            shake.m_AmplitudeGain = Mathf.Lerp(shakeStrength, 0f, i/shakeTimer);
            yield return null;
        }
        shake.m_AmplitudeGain = 0f;
    }
}
