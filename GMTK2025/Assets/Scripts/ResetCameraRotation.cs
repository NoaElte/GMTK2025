using Cinemachine;
using UnityEngine;

public class ResetCameraRotation : MonoBehaviour
{
    private CinemachinePOV cameraPOV;

    private float starterHorizontalRotation;
    private float starterVerticalRotation;

    private void Awake()
    {
        cameraPOV = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();
        starterHorizontalRotation = cameraPOV.m_HorizontalAxis.Value;
        starterVerticalRotation = cameraPOV.m_VerticalAxis.Value;
    }

    private void OnEnable()
    {
        GameManager.OnReset += ResetRotation;
    }

    private void OnDisable()
    {
        GameManager.OnReset -= ResetRotation;
    }

    private void ResetRotation()
    {
        cameraPOV.m_HorizontalAxis.Value = starterHorizontalRotation;
        cameraPOV.m_VerticalAxis.Value = starterVerticalRotation;
    }
}
