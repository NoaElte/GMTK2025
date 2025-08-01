using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RotateByAngle : MonoBehaviour
{
    public enum Axis
    {
        X = 0,
        Y = 1,
        Z = 2
    }

    [SerializeField]
    private float angleOfRotation;
    [SerializeField]
    private Axis axis;
    [SerializeField]
    private AnimationCurve rotationCurve;
    [SerializeField]
    private float duration = 1;
    [SerializeField]
    private UnityEvent onRotateStart;
    [SerializeField]
    private UnityEvent onRotateEnd;
    [SerializeField]
    private bool lastRotateWasClockWise = false;

    private bool isRotating = false;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalRotation = transform.rotation;
    }

    private void OnEnable()
    {
        GameManager.OnReset += ResetRotation;
    }

    private void OnDisable()
    {
        GameManager.OnReset -= ResetRotation;
    }

    public void Rotate()
    {
        if (isRotating)
            return;

        if (lastRotateWasClockWise)
            RotateCounterClockwise();
        else
            RotateClockWise();
    }

    public void RotateClockWise()
    {
        if (isRotating)
            return;

        StartCoroutine(RotateOverTime(angleOfRotation));
        lastRotateWasClockWise = true;
    }

    public void RotateCounterClockwise()
    {
        if (isRotating)
            return;

        StartCoroutine(RotateOverTime(-angleOfRotation));
        lastRotateWasClockWise = false;
    }

    private IEnumerator RotateOverTime(float angleOfRotation)
    {
        isRotating = true;

        onRotateStart?.Invoke();

        Quaternion startRot = transform.rotation;
        Quaternion endRot;
        switch (axis)
        {
            case Axis.X:
                endRot = startRot * Quaternion.Euler(-angleOfRotation, 0, 0);
                break;
            case Axis.Y:
                endRot = startRot * Quaternion.Euler(0, -angleOfRotation, 0);
                break;
            case Axis.Z:
                endRot = startRot * Quaternion.Euler(0, 0, -angleOfRotation);
                break;
            default:
                endRot = startRot * Quaternion.Euler(0, 0, 0);
                break;
        }

        float time = 0;

        while (time < duration)
        {
            float t = time / duration;
            float curveValue = rotationCurve.Evaluate(t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, curveValue);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;

        onRotateEnd?.Invoke();

        isRotating = false;
    }

    private void ResetRotation()
    {
        transform.rotation = originalRotation;
    }
}
