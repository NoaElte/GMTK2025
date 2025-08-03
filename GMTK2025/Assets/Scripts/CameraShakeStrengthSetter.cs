using Cinemachine;
using UnityEngine;

public class CameraShakeStrengthSetter : MonoBehaviour
{
    [SerializeField]
    private float localShakeStrengthMultiplier = .5f;

    private CinemachineBasicMultiChannelPerlin cameraNoise;

    private void Awake()
    {
        cameraNoise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        GameManager.OnShakeStrengthSet += SetStrength;
        GameManager.OnReset += ResetShake;
    }

    private void OnDisable()
    {
        GameManager.OnShakeStrengthSet -= SetStrength;
        GameManager.OnReset -= ResetShake;
    }

    private void SetStrength(float strength)
    {
        cameraNoise.m_AmplitudeGain = strength * localShakeStrengthMultiplier;
    }

    private void ResetShake()
    {
        cameraNoise.m_AmplitudeGain = 0;
    }
}
